import {Component} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { asapScheduler, combineLatest, EMPTY, of, Subject } from 'rxjs';
import { catchError, map, observeOn, startWith, switchMap } from 'rxjs/operators';
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
      if (p) {
        return this.apiService.AddPerson('person', p).pipe(
          map(x => x?.response),
          catchError(e => {
            console.log(e);
            return of(`Api Error:${JSON.stringify(e)}`);
        }))
      }

      return of(null);
    }),
    catchError(() => EMPTY)
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

    combineLatest([this.personForm.get('id')?.valueChanges.pipe(startWith([null])),
                   this.personForm.get('firstName')?.valueChanges.pipe(startWith([null])),
                   this.personForm.get('lastName')?.valueChanges.pipe(startWith([null])),
                   this.personForm.get('dob')?.valueChanges.pipe(startWith([null]))]).pipe(
      observeOn(asapScheduler)
      ).subscribe(() => {
        this.savePerson.next(undefined);
      }
    );
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