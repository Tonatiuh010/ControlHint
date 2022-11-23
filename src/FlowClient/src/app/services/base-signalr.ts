import { InvokeFunctionExpr } from "@angular/compiler";
import * as signalR from "@microsoft/signalr"
import { IHubAction } from "src/interfaces/hubAction";

export class SignalRService {

  private connection: signalR.HubConnection;
  // private actions: IHubAction[];
  private onConnected : () => void;

  id?: string | null;

  constructor(
    url: string,
    // actions: IHubAction[],
    onConnected : () => void = () => this.logHub('Connection started')
  ) {
    // this.actions = actions;
    this.onConnected = onConnected;
    this.connection = new signalR.HubConnectionBuilder()
    .withUrl(url)
    .withAutomaticReconnect()
    .build();

    this.connection.start()
    .then(() => {
      this.logHub('Connection started');
      this.id = this.connection.connectionId;
    })
    .catch(err => this.logHub('Error while starting connection. ', err));

    this.connection.onreconnected(this.onConnected);

  }

  invoke(actionName: string, ...args: any[]) {
    this.connection.invoke(actionName, ...args);
  }

  // bindActions(): void {
  //   this.actions.forEach( a => this.bindAction(a.actionName, a.action));
  // }

  bindAction(actionName : string, fn: (...args : any[]) => any ) : void {
    this.connection.on(actionName, fn);
  }

  close() {
    this.connection.stop();
  }

  logHub(msg: string, ...args: any[]) {
    console.log(`[${new Date(Date.now()).toLocaleTimeString()}]: ${msg}`, ...args);
  }
}
