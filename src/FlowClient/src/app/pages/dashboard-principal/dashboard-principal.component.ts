import { Component, OnInit, ElementRef } from '@angular/core';

@Component({
  selector: 'app-dashboard-principal',
  templateUrl: './dashboard-principal.component.html',
})
export class DashboardPrincipalComponent implements OnInit {

  constructor(private elementRef: ElementRef) { }

  ngOnInit(): void {

    var s = document.createElement("script");
    s.type = "text/javascript";
    s.src = "../assets/js/main.js";
    this.elementRef.nativeElement.appendChild(s);
  }

}
