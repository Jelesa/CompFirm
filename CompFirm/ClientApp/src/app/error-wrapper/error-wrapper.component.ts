import { Component, OnInit } from '@angular/core';
import { ErrorService } from "../shared/error.service";

@Component({
  selector: 'app-error-wrapper',
  templateUrl: './error-wrapper.component.html',
  styleUrls: ['./error-wrapper.component.scss']
})
export class ErrorWrapperComponent implements OnInit {

  constructor(private errorService: ErrorService) { }

  ngOnInit() {
  }

  getErrors() {
    return this.errorService.getErrors();
  }

}
