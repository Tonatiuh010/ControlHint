import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PagesDeviceComponent } from './pages-device.component';

describe('PagesDeviceComponent', () => {
  let component: PagesDeviceComponent;
  let fixture: ComponentFixture<PagesDeviceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PagesDeviceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PagesDeviceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
