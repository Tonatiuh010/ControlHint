import { InvokeFunctionExpr } from "@angular/compiler";
import * as signalR from "@microsoft/signalr"
import { IHubAction } from "src/interfaces/hubAction";

export class SignalRService {

  private connection: signalR.HubConnection;
  private actions: IHubAction[];
  private onConnected : () => void;

  id: string | null;

  constructor(
    url: string,
    actions: IHubAction[],
    onConnected : () => void = () => this.logHub('Connection started')
  ) {
    this.actions = actions;
    this.onConnected = onConnected;
    this.connection = new signalR.HubConnectionBuilder()
    .withUrl(url)
    .withAutomaticReconnect()
    .build();

    this.connection.start()
    .then(() => this.logHub('Connection started'))
    .catch(err => this.logHub('Error while starting connection. ', err));

    this.connection.onreconnected(this.onConnected);

    this.id = this.connection.connectionId;

  }

  invoke(actionName: string, ...args: any[]) {
    this.connection.invoke(actionName, ...args);
  }

  bindActions(): void {
    this.actions.forEach( a => {
      this.connection.on(a.actionName, a.action);
    });
  }

  close() {
    this.connection.stop();
  }

  logHub(msg: string, ...args: any[]) {
    console.log(`[${Date.now.toString()}]: ${msg}`, ...args);
  }
}
