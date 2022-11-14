import { Injectable } from '@angular/core';
import { SignalRService } from './base-signalr';
import { C } from 'src/interfaces/constants';
import { IHubAction, InstanceAction } from 'src/interfaces/hubAction';

@Injectable({
  providedIn: 'root'
})
export class DeviceHubService {
  private static constants = C.hubs.device;
  private static methods = DeviceHubService.constants.methods

  private hub : SignalRService;

  private subInfo : IHubAction = InstanceAction(DeviceHubService.methods.Info);
  private subMonitor : IHubAction = InstanceAction(DeviceHubService.methods.Monitor);
  private subSignal : IHubAction = InstanceAction(DeviceHubService.methods.Signal);

  constructor() {
    this.hub = new SignalRService(
      DeviceHubService.constants._url,
      [
        this.subInfo,
        this.subMonitor,
        this.subSignal
      ]
    );
  }

  setSubInfo(fn: (...args: any[]) => any ) {
    this.subInfo.action = fn;
  }

  setSubMonitor(fn: (...args: any[]) => any ) {
    this.subMonitor.action = fn;
  }

  setSubSignal(fn: (...args: any[]) => any ) {
    this.subSignal.action = fn;
  }

  addToGroup(groupName : string) {
    // Add logs
    this.hub.invoke(DeviceHubService.methods.AddToGroup, groupName);
  }

  removeFromGroup(groupName : string) {
    // add logs
    this.hub.invoke(DeviceHubService.methods.RemoveFromGroup, groupName);
  }

}
