import { Component, OnInit, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common'
import { AuthGuard } from 'src/app/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit {

  constructor(@Inject(DOCUMENT) private document: Document, private authService: AuthGuard) { }

  ngOnInit(): void {
  }

  sidebarToggle()
  {
    this.document.body.classList.toggle('toggle-sidebar');
  }

  logout() {
    this.authService.logout();
  }
}
