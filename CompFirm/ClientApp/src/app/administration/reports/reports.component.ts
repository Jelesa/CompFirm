import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  priceListPdf() {
    window.open('/api/reports/prices/pdf', "_blank");
  }

  priceListCsv() {
    window.open('/api/reports/prices/csv', "_blank");
  }

  ostatkiPdf() {
    window.open('/api/reports/ostatki/pdf', "_blank");
  }

  ostatkiCsv() {
    window.open('/api/reports/ostatki/csv', "_blank");
  }

}
