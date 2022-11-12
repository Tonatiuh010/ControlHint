
interface IHubAction {
  actionName: string;
  action: (...args: any[]) => any;
};

function InstanceAction(
  methodName: string,
  fn: (...args: any[]) => any = () => {}
) : IHubAction {
  return {
    actionName: methodName,
    action: fn
  }
}

export { IHubAction, InstanceAction  }
