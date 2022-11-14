import { Component, OnInit,ElementRef } from '@angular/core';

@Component({
  selector: 'app-pages-device',
  templateUrl: './pages-device.component.html',
  styleUrls: ['./pages-device.component.css']
})
export class PagesDeviceComponent implements OnInit {

  constructor(private elementRef: ElementRef) { }

  ngOnInit(): void {

    var s = document.createElement("script");
    s.type = "text/javascript";
    s.src = "../assets/js/main.js";
    this.elementRef.nativeElement.appendChild(s);
  }

}
