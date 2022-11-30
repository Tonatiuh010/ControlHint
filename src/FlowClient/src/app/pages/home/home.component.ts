import { Component, OnInit, ElementRef, Output, EventEmitter } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { DocumentService } from 'src/app/services/requests/document.service';
import { Device, parseDevice } from 'src/interfaces/catalog/Device';
import { Document as _Doc } from 'src/interfaces/document/Document';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  documents?: _Doc[];

  constructor(
    private documetService: DocumentService
  ) {
  }

  ngOnInit(): void {
    this.documetService.getDocuments(
      docs => this.documents = docs
    );
  }

}
