
using Engine.BO;
using Engine.Interfaces;
using Engine.Services;
using Engine.BO.FlowControl;
using Engine.Constants;
using Org.BouncyCastle.Security;
using System.Data;
using MySql.Data.MySqlClient;
using Engine.BL.Actuators2;
using Google.Protobuf.WellKnownTypes;
using System.Net.NetworkInformation;
using Engine.BO.AccessControl;
using System.Net;
using Engine.BL.Actuators;

namespace Engine.DAL
{
    public class FlowControlDAL : BaseDAL
    {
        public delegate void DALCallback(FlowControlDAL dal);

        private static ConnectionString? _ConnectionString => ConnectionString.InstanceByName(C.HINT_DB);
        public static AccessControlDAL Instance => new();

        public FlowControlDAL() : base(_ConnectionString) { }

        public List<Flow> GetFlows(int? id, int? flowDetailId, string? flowName)
        {
            List<Flow> model = new();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_FLOW, CommandType.StoredProcedure);
                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_FLOW_ID", id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_FLOW_NAME", flowName, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_FLOW_DET", flowDetailId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    int flowId = Validate.getDefaultIntIfDBNull(reader["FLOW_ID"]);
                    Flow? flow = model.Find(x => x.Id == flowId);

                    if (flow == null) 
                    {
                        flow = new Flow()
                        {
                            Id = flowId,
                            Name = Validate.getDefaultStringIfDBNull(reader["FLOW_NAME"]),
                        };

                        model.Add(flow);
                    }

                    if(flow != null)
                    {
                        flow.Steps.Add(new Step()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["FLOW_DET_ID"]),
                            Description = Validate.getDefaultStringIfDBNull(reader["DESCRIPTION"]),
                            Endpoint = new()
                            {
                                Id = Validate.getDefaultIntIfDBNull(reader["ENDPOINT_ID"]),
                                Route = Validate.getDefaultStringIfDBNull(reader["ENDPOINT"]),
                                RequestType = Validate.getDefaultStringIfDBNull(reader["REQUEST_TYPE"]),
                                Api = new API()
                                {
                                    Id = Validate.getDefaultIntIfDBNull(reader["API_ID"]),
                                    Url = Validate.getDefaultStringIfDBNull(reader["URL_BASE"])
                                }                                
                            },
                            IsRequired = Validate.getDefaultBoolIfDBNull(reader["IS_REQUIRED"]),
                            Sequence = Validate.getDefaultIntIfDBNull(reader["SEQUENCE"])
                        });
                    }

                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("FlowControl.GetFlows", msg, ex));

            return model;
        }

        public List<Step> GetSteps(int? stepId, int? parameterId, int? apiId, int? flowId)
        {
            var model = new List<Step>();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_FLOW_PARAMETERS, CommandType.StoredProcedure);
                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_PARAMETER_ID", parameterId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_FLOW_DET_ID", stepId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_API_ID", apiId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_FLOW_ID", flowId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Validate.getDefaultIntIfDBNull(reader["FLOW_DET_ID"]);
                    Step? step = model.Find(x => x.Id == id);

                    if (step == null)
                    {
                        step = new()
                        {
                            Id = id,
                            Sequence = Validate.getDefaultIntIfDBNull(reader["SEQUENCE"]),
                            IsRequired = Validate.getDefaultBoolIfDBNull(reader["FLOW_DETAIL_REQUIRED"]),
                            Description = Validate.getDefaultStringIfDBNull(reader["FLOW_DETAIL_DESCRIPTION"]),
                            Endpoint = new Endpoint()
                            {
                                Id = Validate.getDefaultIntIfDBNull(reader["ENDPOINT_ID"]),
                                RequestType = Validate.getDefaultStringIfDBNull(reader["REQUEST_TYPE"]),
                                Route = Validate.getDefaultStringIfDBNull(reader["ENDPOINT"]),
                                Api = new API()
                                {
                                    Id = Validate.getDefaultIntIfDBNull(reader["API_ID"])
                                },
                            }
                        };

                        model.Add(step);
                    }

                    if (step != null)
                    {
                        step.Endpoint.Params.Add(new Parameter()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["PARAMETER_ID"]),
                            Description = Validate.getDefaultStringIfDBNull(reader["DESCRIPTION"]),
                            ContentType = Validate.getDefaultStringIfDBNull(reader["TYPE"]),
                            IsRequired = Validate.getDefaultBoolIfDBNull(reader["URL_ENDPOINT_REQUIRED"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["PARAMETER"])
                        });
                    }

                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("FlowControl.GetSteps", msg, ex));

            return model;
        }

        public List<API> GetAPIs(int? id)
        {
            List<API> model = new List<API>();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand($"SELECT * FROM CON_API{( id != null? $" WHERE API_ID = {id}" : "" )}", CommandType.Text);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {                                                            
                    model.Add(new API()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["API_ID"]),
                        Url = Validate.getDefaultStringIfDBNull(reader["URL_BASE"]),
                        Description = Validate.getDefaultStringIfDBNull(reader["DESCRIPTION"])
                    });                    
                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("FlowControl.GetAPIs", msg, ex));

            return model;
        }

        public List<Parameter> GetParameters(int? id, int? endpointId)
        {
            List<Parameter> model = new ();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand( QueryParamter(id, endpointId) , CommandType.Text);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Add(new ()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["PARAMETER_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["PARAMETER"]),
                        ContentType = Validate.getDefaultStringIfDBNull(reader["TYPE"]),                        
                        IsRequired = Validate.getDefaultBoolIfDBNull(reader["IS_REQUIRED"]),                        
                        Description = Validate.getDefaultStringIfDBNull(reader["DESCRIPTION"])
                    });
                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("FlowControl.GetParameters", msg, ex));

            return model;
        }

        public List<Device> GetDevices(int? id, string? name, string? _model, string? ip)
        {
            List<Device> model = new();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_DEV_CONNECTION, CommandType.StoredProcedure);
                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DEVICE_ID", id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DEVICE_NAME", name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DEVICE_MODEL", _model, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_IP_ADDRESS", ip, MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["DEVICE_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["DEVICE_NAME"]),
                        Ip = Validate.getDefaultStringIfDBNull(reader["IP"]),
                        Model = Validate.getDefaultStringIfDBNull(reader["DEVICE_MODEL"]),
                        IsActive = Validate.getDefaultBoolIfDBNull(reader["IS_ACTIVE"]),
                        LastUpdate = Validate.getDefaultDateIfDBNull(reader["LAST_UPDATE"])
                    });
                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("FlowControl.GetDevices", msg, ex));

            return model;
        }

        public int GetDeviceId(string deviceName)
        {
            int id = 0;

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(QueryGetDeviceId(deviceName), CommandType.Text);

                var result = cmd.ExecuteScalar();

                if(result != null)
                {
                    _ = int.TryParse(result.ToString(), out id);
                }
                
            }, (ex, msg) => SetExceptionResult("FlowControl.GetDeviceId", msg, ex));

            return id;
        }

        public int GetDeviceFlow(int? deviceId, int? flowId)
        {
            int model = 0;

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(QueryDeviceFlow(deviceId, flowId), CommandType.Text);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())                
                    model = Validate.getDefaultIntIfDBNull(reader["FLOW_ID"]);
                
                reader.Close();
            }, (ex, msg) => SetExceptionResult("FlowControl.GetDeviceFlow", msg, ex));

            return model;
        }

        public List<DeviceHintConfig> GetDeviceEmployeeHints(int? deviceId, int? employeeId, int? hintKey)
        {
            List<DeviceHintConfig> model = new();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(QueryHintKeyDevice(deviceId, employeeId, hintKey), CommandType.Text);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Add(new(
                        Validate.getDefaultIntIfDBNull(reader["DEVICE_ID"]),
                        Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"]),
                        Validate.getDefaultIntIfDBNull(reader["HINT_KEY_ID"])
                    ));
                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("FlowControl.GetDeviceEmployeeHints", msg, ex));

            return model;
        }

        public Result SetDeviceEmployeeHint(int deviceId, int employeeId, int hintKey, string txnUser)
        {
            Result result = new();
            string sSp = SQL.SET_HINT_CONFIG;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DEVICE_ID", deviceId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE_ID", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_HINT_KEY", hintKey, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            },
                (ex, msg) => SetExceptionResult("FlowControlDAL.SetDeviceEmployeeHint", msg, ex, result)
            );

            return result;
        }

        public ResultInsert SetDevice(Device device, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_DEV_CONNECTION;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DEVICE_ID", device.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DEVICE_NAME", device.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DEVICE_MODEL", device.Model, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_IP_ADDRESS", device.Ip, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            },
                (ex, msg) => SetExceptionResult("FlowControlDAL.SetDevice", msg, ex, result),
                () => SetResultInsert(result, device)
            );

            return result;
        }

        public ResultInsert SetAPI(API api, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_API;

            int id = 0;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter apiId = CreateParameterOut("OUT_API_ID", MySqlDbType.Int32);
                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_API_ID", api.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_URL", api.Url, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DESCRIPTION", api.Description, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(apiId);
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

                id = Validate.getDefaultIntIfDBNull(apiId.Value);

            },
                (ex, msg) => SetExceptionResult("FlowControlDAL.SetAPI", msg, ex, result),
                () => {                     
                    SetResultInsert(result, api);
                    //if(result?.InsertDetails?.Id != id)
                    //{
                    //    throw new Exception("Ids are not the same!! FlowControlDAL.SetAPI()");
                    //}
                }
            );

            return result;
        }

        public ResultInsert SetEndpoint(Endpoint endpoint, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_URL_ENDPOINT;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_ENDPOINT_ID", endpoint.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_ENDPOINT", endpoint.Route, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_TYPE", endpoint.RequestType, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_API_ID", endpoint.Api.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("FlowControlDAL.SetEndpoint", msg, ex, result),
                () => SetResultInsert(result, endpoint)
            );

            return result;
        }

        public ResultInsert SetParameter(Parameter parameter, int endpointId, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_ENDPOINT_PARAMETER;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_PARAMETER_ID", parameter.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_PARAMETER", parameter.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_TYPE", parameter.ContentType, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_REQUIRED", parameter.IsRequired, MySqlDbType.Int16));
                cmd.Parameters.Add(CreateParameter("IN_DESCRIPTION", parameter.Description, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_ENDPOINT_ID", endpointId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("FlowControlDAL.SetParameter", msg, ex, result),
                () => SetResultInsert(result, parameter)
            );

            return result;
        }

        public Result SetDevFlow(int deviceId, int flowId, string txnUser)
        {
            Result result = new ();

            string sSp = SQL.SET_DEV_FLOW;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DEVICE_ID", deviceId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_FLOW_ID", flowId, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("FlowControlDAL.SetDevFlow", msg, ex, result)
            );

            return result;

        }

        #region Crazy Queries
        private static string QueryDeviceFlow(int? deviceId = null, int? flowId = null)
        {
            string condition =
                $" WHERE FLOW_ID = IFNULL({(flowId != null ? flowId : "null")}, FLOW_ID) " +
                $"AND DEVICE_ID = IFNULL({(deviceId != null ? deviceId: "null")}, DEVICE_ID )"
            ;

            return @"                
                SELECT 
                    * 
                FROM 
                    DEV_FLOW
            " + condition;
        }

        private static string QueryParamter(int? id = null, int? endpointId = null)
        {
            string condition = 
                $" WHERE PARAMETER_ID = IFNULL({ (id != null? id : "null") }, PARAMETER_ID) " +
                $"AND ENDPOINT_ID = IFNULL({ (endpointId != null? endpointId : "null" ) }, ENDPOINT_ID )"
            ;

            return @"                
                SELECT 
                    * 
                FROM 
                    CON_ENDPOINT_PARAMETER
            " + condition;
        }

        private static string QueryHintKeyDevice(int? deviceId, int? employeeId, int? hintKey)
        {
            string condition =
                $" WHERE DEVICE_ID = IFNULL({(deviceId != null ? deviceId : "null")}, DEVICE_ID) " +
                $"AND HINT_KEY_ID = IFNULL({(hintKey != null ? hintKey : "null")}, HINT_KEY_ID) " +
                $"AND EMPLOYEE_ID = IFNULL({(employeeId != null ? employeeId : "null")}, EMPLOYEE_ID)";

            return @"
                SELECT
                    *
                FROM
                    DEV_HINT_CONFIG
            " + condition;
        }

        private static string QueryGetDeviceId(string deviceName)
        {
            return $"SELECT GET_DEVICE('{deviceName}') AS DEVICE_ID";
        }

        #endregion
    }
}
