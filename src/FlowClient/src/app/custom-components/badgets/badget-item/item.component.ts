import { Component, OnInit, ElementRef, Output, EventEmitter, Type, Input, OnChanges, SimpleChanges } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { Device, parseDevice } from 'src/interfaces/catalog/Device';
import { Transaction as Txn} from 'src/interfaces/document/Transaction'
import { Document as _Doc, ItemQuotation, QuotationParameters, SaleParameters } from 'src/interfaces/document/Document'
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { C } from 'src/interfaces/constants';
import { DocumentService } from 'src/app/services/requests/document.service';
import { FormControl } from '@angular/forms';
import { parseDocument } from 'src/interfaces/document/Document';
import { Department } from 'src/interfaces/catalog/Department';


@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
})
export class ItemComponent implements OnInit, OnChanges {
  @Input() item? : ItemQuotation;
  @Input() isEditable?: boolean;
  @Output() onItem = new EventEmitter<ItemQuotation>();
  @Output() onDeleteItem = new EventEmitter<ItemQuotation>();

  id: FormControl = new FormControl();
  name: FormControl = new FormControl();
  code: FormControl = new FormControl();

  description: FormControl = new FormControl();
  value: FormControl = new FormControl();
  qty: FormControl = new FormControl();

  total: FormControl = new FormControl();

  showFeedback: boolean = false;
  displayItem: boolean = true;

  constructor() { }

  ngOnInit(): void {
    this.prepareForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.prepareForm();
  }

  onChangePrice() {
    this.total.setValue(this.calcTotal());
  }

  onName() {
    if(this.item){
      this.item.name = this.name.value;
    }
  }

  save() {
    if(this.validateForm()) {
      this.onItem.emit(this.item);
    }
  }

  deleteItem() {
    this.onDeleteItem.emit(this.item);
  }

  showItem() {
    if(this.displayItem) {
      this.displayItem = false;
    } else {
      this.displayItem = true;
    }
  }

  private setForm(item: ItemQuotation) {

    this.id.setValue(item.id);
    this.name.setValue(item.name);
    this.code.setValue(item.code);
    this.description.setValue(item.description);
    this.value.setValue(item.value);
    this.qty.setValue(item.qty);
    this.total.setValue(this.calcTotal());

  }

  private validateForm() : boolean {
    if(!(
      this.id.value &&
      this.name.value &&
      this.code.value &&
      this.description.value &&
      this.value.value &&
      this.qty.value &&
      this.total.value
    )) {
      this.showFeedback = true;
      return false;
    } else {
      this.showFeedback = false;
      return true;
    }
  }

  private calcTotal() {
    let qty = +this.qty.value;
    let value = +this.value.value;

    if(qty && value) {
      return qty * value;
    } else return 0

  }

  private cleanForm() {
    this.id.setValue('');
    this.name.setValue('');
    this.code.setValue('');
    this.description.setValue('');
    this.value.setValue(1);
    this.qty.setValue('');
    this.total.disable();
    this.total.setValue(1);
  }

  private disableForm() {
    this.id.disable();
    this.name.disable();
    this.code.disable();
    this.description.disable();
    this.value.disable();
    this.qty.disable();
    this.total.disable();
    this.total.disable();
  }

  private prepareForm() {
    this.cleanForm();

    if(this.item) {
      this.setForm(this.item);
      this.id.disable();
    }

    if(!this.isEditable){
      this.disableForm();
    }
  }

}
