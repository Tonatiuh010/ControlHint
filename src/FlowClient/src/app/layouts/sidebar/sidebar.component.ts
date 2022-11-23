import { Component, OnInit } from '@angular/core';
import { C } from 'src/interfaces/constants';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent implements OnInit {
  isAdmin : boolean = false;

  constructor() { }

  ngOnInit(): void {
    let type = localStorage.getItem("type");
    this.isAdmin = type == C.roles.Admin;
  }

}
