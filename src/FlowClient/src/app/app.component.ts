import { Component ,ElementRef} from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  constructor(private elementRef: ElementRef,  public  _router: Router) { }

  ngOnInit() {
  }
}
