
interface IHubAction {
  actionName: string;
  action: (...args: any[]) => any;
};

export { IHubAction }
