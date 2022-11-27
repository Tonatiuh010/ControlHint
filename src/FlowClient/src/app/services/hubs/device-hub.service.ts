import { Injectable } from '@angular/core';
import { SignalRService } from '../base-signalr';
import { C } from 'src/interfaces/constants';
import { IHubAction, InstanceAction } from 'src/interfaces/hubAction';

@Injectable({
  providedIn: 'root'
})
export class DeviceHubService {
  private static constants = C.hubs.device;
  private static constant = C.hubs.check;
  private static methods = DeviceHubService.constants.methods
  private static method = DeviceHubService.constant.methods

  private hub : SignalRService;

  constructor() {
    this.hub = new SignalRService(DeviceHubService.constants._url);
  }

  // args: type: string, msg: string
  setSubInfo(fn: (...args: any[]) => any ) {
    this.hub.bindAction(DeviceHubService.methods.Info, fn);
  }

  setSubMonitor(fn: (...args: any[]) => any ) {
    this.hub.bindAction(DeviceHubService.methods.Monitor, fn);
  }

  setSubSignal(fn: (...args: any[]) => any ) {
    this.hub.bindAction(DeviceHubService.methods.Signal, fn)

  }

  addToGroup(groupName : string) {
    this.hub.logHub(`${this.hub.id} - Subscribed to ${groupName}`);
    this.hub.invoke(DeviceHubService.methods.AddToGroup, groupName);
  }

  removeFromGroup(groupName : string) {
    // add logs
    this.hub.invoke(DeviceHubService.methods.RemoveFromGroup, groupName);
  }

}
