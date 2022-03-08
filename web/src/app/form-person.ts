import {Component} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { of, Subject } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { ApiService } from './api.service';
import { Person } from './models/person.model';

@Component({
  selector: 'form-person',
  templateUrl: 'form-person.html',
})
export class FormPerson {

  personForm: FormGroup;
  savePerson = new Subject<Person>();
  result$ = this.savePerson.asObservable().pipe(
    switchMap((p) => {
      return this.apiService.AddPerson('person', p).pipe(map(x => x?.response))
    }),
    catchError(e => of(`Api Error:${e}`))
  );
  constructor(private apiService: ApiService) { }

  ngOnInit() {
    this.personForm = new FormGroup({
      id: new FormControl('1'),
      firstName: new FormControl('', [
        Validators.required,
        Validators.maxLength(255)]),
      lastName: new FormControl('', [
        Validators.required,
        Validators.maxLength(255)]),
      dob: new FormControl('', [
        Validators.required,]),
    });
  }

  getFirstName() {
    return this.personForm.get('firstName');
  }

  getLastName() {
    return this.personForm.get('lastName');
  }

  save() {
    if (this.personForm.valid) {
      const person: Person = this.convertFormModelToPayload(this.personForm.value);
      console.log(person);
      this.savePerson.next(person);
      console.log('Save with no errors!');
    }
  }

  convertFormModelToPayload(model: any) {
    return {...model, id : +model.id} as Person
  }
}
