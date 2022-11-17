using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.BO;
using Engine.Constants;
using Engine.BO.FlowControl;
using Engine.DAL;

namespace Engine.BL.Actuators2
{
    public class FlowBL : BaseBL<FlowControlDAL>
    {
        public List<Flow> GetFlows(int? id = null, int? flowDetailId = null, string? name = null)
        {
            var flows = Dal.GetFlows(id, flowDetailId, name);

            foreach (var f in flows)
                CompleteFlow(f);

            return flows;
        }

        public ResultInsert SetEndpoint(Endpoint endpoint) => Dal.SetEndpoint(endpoint, C.GLOBAL_USER);

        public Flow? GetFlow(int id) => GetFlows(id).FirstOrDefault();

        public Flow? GetFlowByDevice(int deviceId)
        {
            int flowId = Dal.GetDeviceFlow(deviceId, null);
            return GetFlow(flowId);
        }

        public Flow? GetFlowByDeviceName(string deviceName)
        {
            int flowId = GetDeviceFlowId(deviceName);            
            return GetFlow(flowId);
        }

        private int GetDeviceFlowId(string deviceName) => Dal.GetDeviceFlow(
           Dal.GetDeviceId(deviceName),
           null
       );

        public List<Step> GetSteps(int? id = null, int? parameterId = null, int? apiId = null, int? flowId = null)
        {
            var steps = Dal.GetSteps(id, parameterId, apiId, flowId);

            foreach (var step in steps)
                CompleteStep(step);

            return steps;
        }

        public Step? GetStep(int id) => GetSteps(id).FirstOrDefault();

        public List<API> GetAPIs(int? id = null) => Dal.GetAPIs(id);

        public API? GetAPI(int id) => GetAPIs(id).FirstOrDefault();

        public ResultInsert SetAPI(API api) => Dal.SetAPI(api, C.GLOBAL_USER);

        public List<Parameter> GetParameters(int? id = null, int? endpointId = null) 
            => Dal.GetParameters(id, endpointId);

        public Parameter? GetParameter(int id) => GetParameters(id).FirstOrDefault();

        public ResultInsert SetParameter(Parameter parameter, int endpointId) => Dal.SetParameter(parameter, endpointId, C.GLOBAL_USER);

        public Result SetDevFlow(int deviceId, int flowId) => Dal.SetDevFlow(deviceId, flowId, C.GLOBAL_USER);

        #region Complete
        private void CompleteFlow(Flow flow)
        {
            if(flow.Steps.Count == 0)            
                flow.Steps = GetSteps(flowId: flow.Id);
            
            if(flow.Steps.Count > 0)
            {
                foreach(var step in flow.Steps)                
                    CompleteStep(step);                
            }
        }

        private void CompleteStep(Step step)
        {            
            CompleteEndpoint(step.Endpoint);
        }

        private void CompleteEndpoint(Endpoint endpoint) 
        {
            if(endpoint.Api != null && endpoint.Api.IsValid())            
                endpoint.Api = GetAPI((int)endpoint.Api.Id);

            if (endpoint?.Params.Count == 0)
                endpoint.Params = GetParameters(endpointId: endpoint.Id);
        }
        #endregion
    }
}
