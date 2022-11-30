import { Component, OnInit, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { Flow } from 'src/interfaces/api/Api';
import { Device } from 'src/interfaces/catalog/Device';
import { FormControl } from '@angular/forms';
import { DataBody } from 'src/interfaces/catalog/DataBody';
import { C } from 'src/interfaces/constants';
import { ItemQuotation, QuotationParameters, SaleParameters } from 'src/interfaces/document/Document';
import { Transaction } from 'src/interfaces/document/Transaction';
import { DocumentService } from 'src/app/services/requests/document.service';
import { ThisReceiver } from '@angular/compiler';
import { TitleStrategy } from '@angular/router';

@Component({
  selector: 'document-detail',
  templateUrl: 'document-detail.component.html',
})
export class DocumentDetailComponent implements OnInit,  OnChanges {
  @Input() isEditable : boolean = true;
  @Input() type? : string;
  @Input() parameters? : QuotationParameters | SaleParameters;
  @Output() onTransaction = new EventEmitter<QuotationParameters | SaleParameters>();

  showFeedback : boolean = false;

  quoParams?: QuotationParameters;
  saleParams?: SaleParameters;

  // Form Controls

  documentName: FormControl = new FormControl();
  date: FormControl = new FormControl();

  // Sales Parameters!!
  place: FormControl = new FormControl();
  item: FormControl = new FormControl();
  salesAddress: FormControl = new FormControl();
  salesNum: FormControl = new FormControl();
  salesPlace: FormControl = new FormControl();
  salesSign: FormControl = new FormControl();

  customerAddress: FormControl = new FormControl();
  customerNum: FormControl = new FormControl();
  customerPlace: FormControl = new FormControl();
  customerSign: FormControl = new FormControl();

  law: FormControl = new FormControl();
  total: FormControl = new FormControl();

  // Quotation Parameters!!
  duoDate: FormControl = new FormControl();
  quoName: FormControl = new FormControl();
  client: FormControl = new FormControl();
  contact: FormControl = new FormControl();
  notes: FormControl = new FormControl();

  // add items!!!!
  constructor(
    private service : DocumentService
  ) {

  }

  ngOnInit(): void {
    this.preparaComponent();
    this.law.setValue("Ley #100 Venta Justa");
  }

  ngOnChanges() {
    this.preparaComponent();
  }

  addItem() {
    let params : QuotationParameters;

    if(this.quoParams) {
      params = this.quoParams;
    } else {
      params = this.readQuotationForm();
    }

    params.items.push({
      id: this.getItemNextItemId()
    });
    this.quoParams = params;
  }

  save() {
    if(this.isQuotation() && this.validateQuotationForm()) {
      let params = this.readQuotationForm();
      this.onTransaction.emit(params);
      // this.clearForms();
    } else if(this.isSale() && this.validateSaleForm()) {
      let params = this.readSaleForm();
      this.onTransaction.emit(params);
      // this.clearForms();
    }
  }

  isQuotation() {
    return this.type == C.DocumentType.Quotation;
  }

  isSale() {
    return this.type == C.DocumentType.Sale;
  }

  getItemNextItemId() : number {
    if(this.quoParams) {
      return this.quoParams.items.length + 1;
    } else {
      return 1;
    }
  }

  setItem(item: ItemQuotation) {
    if(this.quoParams) {
      let index = this.quoParams.items.findIndex(x => x.id == item.id);

      if(index != -1) {
        this.quoParams.items[index] = item;
      } else {
        this.quoParams.items.push(item);
      }
    }
  }

  removeItem(item: ItemQuotation) {
    if(this.quoParams) {
      this.quoParams.items = this.quoParams.items.filter(x => x.id != item.id)
    }
  }

  private clearForms() {
    this.cleanQuotationForm();
    this.cleanSaleForm();
  }

  private setQuotationForm(parameters: QuotationParameters) {
    var dt = new Date(parameters.date);
    var duoDt = new Date(parameters.duoDate);

    this.type = C.DocumentType.Quotation;
    this.client.setValue(parameters.client);
    this.contact.setValue(parameters.contact);
    this.date.setValue(dt.toISOString().split('T')[0]);
    this.duoDate.setValue(duoDt.toISOString().split('T')[0]);
    this.documentName.setValue(parameters.name);
    this.notes.setValue(parameters.notes);
    this.documentName.setValue(parameters.name);
    this.quoName.setValue(parameters.name);
    // Items???
  }

  private setSaleForm(parameters: SaleParameters) {
    var dt = new Date(parameters.date);
    this.type = C.DocumentType.Sale;
    this.documentName.setValue(parameters.docName);
    this.date.setValue(dt.toISOString().split('T')[0]);
    this.item.setValue(parameters.item);
    this.law.setValue(parameters.law);
    this.customerAddress.setValue(parameters.customerAddress);
    this.customerNum.setValue(parameters.customerNum);
    this.customerPlace.setValue(parameters.place);
    this.customerSign.setValue(parameters.customerSign);
    this.salesAddress.setValue(parameters.salesAddress);
    this.salesNum.setValue(parameters.salesNum);
    this.salesPlace.setValue(parameters.salesPlace);
    this.salesSign.setValue(parameters.salesSign);
    this.law.setValue(parameters.law);
    this.total.setValue(parameters.total);
  }

  private readQuotationForm() {
    this.quoParams = {
      client: this.client.value,
      contact: this.contact.value,
      date: this.date.value,
      duoDate: this.duoDate.value,
      items: this.quoParams?.items as QuotationParameters[],
      name: this.documentName.value,
      notes: this.notes.value,
      docName: this.documentName.value,
    }

    return this.quoParams;
  }

  private readSaleForm() {
    this.saleParams = {
      docName: this.documentName.value,
      date: this.date.value,
      item: this.item.value,
      place: this.law.value,
      customerAddress: this.customerAddress.value,
      customerNum: this.customerNum.value,
      customerName: this.customerPlace.value,
      customerPlace: this.customerPlace.value,
      customerSign: this.customerSign.value,

      salesAddress: this.salesAddress.value,
      salesNum: this.salesNum.value,
      salesPlace: this.salesPlace.value,
      salesSign: this.salesSign.value,

      law: this.law.value,
      total: this.total.value

    }

    return this.saleParams;
  }

  private validateQuotationForm() {
    let ref = this;
    let validateItems = () : boolean => {
      if(ref.quoParams) {
        let items = ref.quoParams.items;
        if(items) {
          let isValid = false;
          items.forEach(x => {
            if(
              x.code &&
              x.description &&
              x.id &&
              x.name &&
              x.qty &&
              x.value
            ) {
              isValid = true;
            } else {
              isValid = false;
              return;
            }
          });

          return isValid;

        } else {
          return false;
        }
      } else {
        return false;
      }
    }

    if(!(
      this.documentName.value &&
      this.date.value &&
      this.duoDate.value  &&
      this.quoName.value &&
      this.client.value &&
      this.contact.value  &&
      this.notes.value &&
      validateItems()
    ))
    {
      this.showFeedback = true;
      return false;
    } else {
      this.showFeedback = false;
      return true;
    }
  }

  private validateSaleForm() {
    if(!(
      this.documentName.value &&
      this.date.value &&
      this.item.value &&
      this.law.value &&
      this.customerAddress.value &&
      this.customerNum.value &&
      this.customerPlace.value &&
      this.customerPlace.value &&
      this.customerSign.value &&
      this.salesAddress.value &&
      this.salesNum.value &&
      this.salesPlace.value &&
      this.salesSign.value &&
      this.law.value &&
      this.total.value
    ))
    {
      this.showFeedback = true;
      return false;
    } else {
      this.showFeedback = false;
      return true;
    }
  }

  private disableQuotationForm() {
    this.documentName.disable()
    this.date.disable()
    this.duoDate.disable()
    this.quoName.disable()
    this.client.disable()
    this.contact.disable()
    this.notes.value.disable()
  }

  private disableSaleForm() {
    this.documentName.disable();
    this.date.disable();
    this.item.disable();
    this.law.disable();
    this.customerAddress.disable();
    this.customerNum.disable();
    this.customerPlace.disable();
    this.customerPlace.disable();
    this.customerSign.disable();
    this.salesAddress.disable();
    this.salesNum.disable();
    this.salesPlace.disable();
    this.salesSign.disable();
    this.law.disable();
    this.total.disable();
  }


  private cleanQuotationForm() {
    this.documentName.setValue('');
    this.date.setValue('');
    this.duoDate.setValue('');
    this.quoName.setValue('');
    this.client.setValue('');
    this.contact.setValue('');
    this.notes.value.setValue('');
  }

  private cleanSaleForm() {
    this.documentName.setValue('');
    this.date.setValue('');
    this.item.setValue('');
    this.law.setValue('');
    this.customerAddress.setValue('');
    this.customerNum.setValue('');
    this.customerPlace.setValue('');
    this.customerPlace.setValue('');
    this.customerSign.setValue('');
    this.salesAddress.setValue('');
    this.salesNum.setValue('');
    this.salesPlace.setValue('');
    this.salesSign.setValue('');
    this.law.setValue('');
    this.total.setValue('');
  }

  private preparaComponent() {
    if (this.parameters) {
      if ('place' in this.parameters) {
        this.saleParams = this.parameters as SaleParameters;
        this.setSaleForm(this.saleParams);

      } else if ('duoDate' in this.parameters) {
        this.quoParams = this.parameters as QuotationParameters;
        this.setQuotationForm(this.quoParams);
      }
    }

    if(!this.isEditable){
      this.disableQuotationForm();
      this.disableSaleForm();
    }
  }
}
