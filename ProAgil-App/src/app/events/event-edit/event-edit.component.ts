import { Events } from './../../models/Events';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { EventService } from 'src/app/services/event/event.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.css']
})
export class EventEditComponent implements OnInit {

  title = 'Editar Evento';
  event: Events;
  registerForm: FormGroup;

  constructor(
    private eventService: EventService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private router: ActivatedRoute) { }

  ngOnInit(): void {
    this.validation();
    this.getEvent();
  }

  validation() {
    this.registerForm = this.fb.group({
      theme: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      eventDate: ['', Validators.required],
      personQtd: ['', [Validators.required, Validators.max(120000), Validators.pattern(/^-?(0|[1-9]\d*)?$/)]],
      contactPhone: ['', Validators.required],
      contactEmail: ['', [Validators.required, Validators.email]],
      imagemURL: [''],
      lots: this.fb.group({
        name: ['', Validators.required],
        qty: ['', Validators.required],
        price: ['', Validators.required],
        startDate: [''],
        endDate: [''],
      }),
      networks: this.fb.group({
        name: ['', Validators.required],
        url: ['', Validators.required],
      })
    });
  }
  getEvent() {
    this.event = new Events();
    const id = +this.router.snapshot.paramMap.get('id'); // o sinal de + converte para number
    this.eventService.getEvent(id)
      .subscribe(
        (event: Events) => {
          this.event = Object.assign({}, event);
          //console.log(this.event); // depois remover
        }
      );
  }

  onFileChange(data: any) { }

}
