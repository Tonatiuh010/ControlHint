import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Flow } from 'src/interfaces/api/Api';
import { Device } from 'src/interfaces/catalog/Device';
import { UserService as service } from '../../../services/requests/user.service';
import { EmployeeService } from '../../../services/requests/employee.service';
import { Form, FormControl } from '@angular/forms';
import { dataBody } from 'src/interfaces/catalog/dataBody';
import { C } from 'src/interfaces/constants';
import { Employee } from 'src/interfaces/catalog/Employee';
import { Department } from "src/interfaces/catalog/Department";
import { CatalogService } from "../../../services/requests/catalog.service";
import { Shift } from "src/interfaces/catalog/Shift";
import { Position } from "src/interfaces/catalog/Position";
import { UserService } from "../../../services/requests/user.service";
import { User } from "src/interfaces/catalog/User";
//IMPORTAR CATALOGOS

@Component({
  selector: 'employee-modal',
  templateUrl: './modal-employee.component.html',
})
export class ModalEmployeeComponent implements OnInit {
  shifts?: Shift[];
  positions?: Position[];
  selectorShift: FormControl = new FormControl();
  selectorPosition: FormControl = new FormControl();
  inputName: FormControl = new FormControl();
  inputLastName: FormControl = new FormControl();
  inputImage: FormControl = new FormControl();
  fileImage: any;
  inputUserName: FormControl = new FormControl();
  inputPassword: FormControl = new FormControl();
  selectorType: FormControl = new FormControl();
  user?: User;
  @Input() employee? : Employee;
  @Output() onCloseModal = new EventEmitter();
  @Output() onEmployeeAction = new EventEmitter<Employee>();

  constructor(
    private service : service,
    private EmployeeService : EmployeeService,
    private CatalogService : CatalogService,
    private UserServer : UserService
  ) { }

  ngOnInit(): void {
    this.CatalogService.getCatalog((shift, position) => {
        this.positions = position;
        this.shifts = shift;
      }
    );
    if(this.employee){
      this.UserServer.getUserId(this.employee?.id as number, user => {
        if(user) {
          this.inputUserName.setValue(user.userName);
          this.inputPassword.setValue(user.password);
          this.selectorType.setValue(user.userType);
        }else{
          this.selectorType.setValue('USER');
        }
      });
    }else{
      this.selectorType.setValue('USER');
    }
  }

  ngOnChanges() {
    if (this.employee) {
      this.inputName.setValue(this.employee.name);
      this.inputLastName.setValue(this.employee.lastName);
      this.selectorPosition.setValue(this.employee.position?.positionId);
      this.selectorShift.setValue(this.employee.shift?.id);
      this.showModal();
    } else {
      this.closeModal();
    }
  }

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.getBase64(file, b64 => {
        this.fileImage = b64;
      })
    }
  }

  cleanUserForm(){
    this.inputUserName.setValue('');
    this.inputPassword.setValue('');
    this.selectorType.setValue('USER');
  }

  setEmployee() {
    if(this.employee){
      let shiftId: number = +this.selectorShift.value;
      let positionId: number = +this.selectorPosition.value;
      let name: string = this.inputName.value;
      let lastName: string = this.inputLastName.value;
      let image: string = this.fileImage;

      let temporalEmployee: Employee = {
        id: this.employee.id,
        name: name,
        lastName: lastName,
        position: {
          positionId: positionId
        },
        shift: {
          id: shiftId
        }
      }
      this.EmployeeService.setEmployee(temporalEmployee, image, data => {
        let result: dataBody = data as dataBody;

        if(result.status == C.keyword.OK){
          // this.employee?.id = temporalEmployee.id;
          // this.
          this.onEmployeeAction.emit(temporalEmployee);
          this.closeModal();
        }
      })
    }

  }

  setUser() {
      let userName: string = this.inputUserName.value;
      let password: string = this.inputPassword.value;
      let type: string = this.selectorType.value;
      let employeeId: number = this.employee?.id as number;

      this.UserServer.setUser({
        userName: userName,
        password: password,
        userType: type,
        id: employeeId
      },
      res => {
        console.log(res);
      }
      )
  }

  showModal() {
    this.triggerBtn("btn-show-modal");
  }

  closeModal() {
    this.triggerBtn("btn-close-modal");
    this.onCloseModal.emit();
    this.cleanUserForm();
  }



  private triggerBtn(btnId : string) {
    let btn = document.getElementById(btnId);

    if (btn) {
      btn.click();
    }

  }

  private getBase64(file: File, fn: (b64: any) => void) {
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
      fn(reader.result);
    };
    reader.onerror = function (error) {
      console.log('Error: ', error);
    };
 }

}
