
using Engine.BO;
using Engine.Interfaces;
using Engine.Services;
using Engine.BO.FlowControl;
using Engine.Constants;
using Org.BouncyCastle.Security;
using System.Data;
using MySql.Data.MySqlClient;

namespace Engine.DAL
{
    public class FlowControlDAL : BaseDAL
    {
        public delegate void DALCallback(FlowControlDAL dal);

        private static ConnectionString? _ConnectionString => ConnectionString.InstanceByName(C.HINT_DB);
        public static AccessControlDAL Instance => new();

        public FlowControlDAL() : base(_ConnectionString) { }

        public List<Flow> GetFlows(int? id, string? flowName, int? flowDetailId)
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
                        model.Add( new Flow()
                        {
                            Id = flowId,
                            Name = Validate.getDefaultStringIfDBNull(reader["FLOW_NAME"]),
                        });
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
                using var cmd = CreateCommand(SQL.GET_FLOW, CommandType.StoredProcedure);
                IDataParameter pResult = CreateParameterOut("OUT_MSG", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_PARAMETER_ID", parameterId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_FLOW_DET_ID", stepId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_API_ID", apiId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_FLOW_ID", flowId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Validate.getDefaultIntIfDBNull(reader["FLOW_ID"]);
                    Step? step = model.Find(x => x.Id == id);

                    if (step == null)
                    {
                        model.Add(new ()
                        {
                            Id = stepId,
                            Sequence = Validate.getDefaultIntIfDBNull(reader["SEQUENCE"]),                            
                            IsRequired = Validate.getDefaultBoolIfDBNull(reader["FLOW_DETAIL_REQUIRED"]),
                            Description = Validate.getDefaultStringIfDBNull(reader["FLOW_DETAIL_DESCRIPTION"]),
                            Endpoint = new Endpoint()
                            {
                                Id = Validate.getDefaultIntIfDBNull(reader["ENDPOINT_ID"]),
                                RequestType = Validate.getDefaultStringIfDBNull(reader["REQUEST_TYPE"]),
                                Route = Validate.getDefaultStringIfDBNull(reader["REQUEST_TYPE"]),
                                Api = new API()
                                {
                                    Id = Validate.getDefaultIntIfDBNull(reader["API_ID"])
                                },
                                
                            }
                        });
                    }

                    if (step != null)
                    {
                        step.Endpoint.Params.Add(new Parameter()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["FLOW_DET_ID"]),
                            Description = Validate.getDefaultStringIfDBNull(reader["DESCRIPTION"]),
                            ContentType = Validate.getDefaultStringIfDBNull(reader["REQUEST_TYPE"]),
                            IsRequired = Validate.getDefaultBoolIfDBNull(reader["URL_ENDPOINT_REQUIRED"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["PARAMETER"])
                        });
                    }

                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("FlowControl.GetSteps", msg, ex));

            return model;
        }

        public List<Device> GetDevices(int? id, string? name, bool? status = true)
        {
            return new List<Device>()
            {
                new () {
                    Id = 1,
                    Ip = "0.0.0.1",
                    Name = "Test 1",
                    Activations = 2,
                    LastUpdate = DateTime.Now,
                    Status = "ENABLED"
                },
                new () {
                    Id = 2,
                    Ip = "255.255.255.0",
                    Name = "Test 2",
                    Activations = 4,
                    LastUpdate = DateTime.Now,
                    Status = "ENABLED"
                },
            };
        }

        public ResultInsert SetDevice(Device device, string txnUser)
        {
            return new ResultInsert() { 
                InsertDetails = new InsertStatus(device)
                {
                    
                }
            };

        }

    }
}
