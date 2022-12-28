import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { RequestsService } from '../../requests.service';
import { RequestCard } from '../../models/request-card.model';
import { BaseReferenceBook } from "../../models/base-reference-book.model";

@Component({
  selector: 'app-request-list',
  templateUrl: './request-list.component.html',
  styleUrls: ['./request-list.component.scss']
})
export class RequestListComponent implements OnInit {

  statuses: BaseReferenceBook[];
  requests: RequestCard[];
  pageCount: number = 0;
  pageNumber: number = 0;
  private limit: number = 15;
  private offset: number = 0;
  searchString: string;
  statusId: number = -1;
  isSearch: boolean = false;

  @Input() adminSearch: boolean = false;
  @Output() goToCardEvent = new EventEmitter<number>()

  constructor(
    private requestsService: RequestsService) { }

  ngOnInit() {

    this.requestsService.getStatuses().subscribe(res => {
      this.statuses = [{ id: -1, name: 'Все' }, ...res];
    });

    this.search();
  }

  getUtcDate(dateString: string) {
    let date = new Date(dateString);

    return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()}`;
  }

  prevPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;

      this.offset = this.pageNumber * this.limit;
      this.search();
    }
  }

  nextPage() {
    if (this.pageNumber < this.pageCount - 1) {
      this.pageNumber++;

      this.offset = this.pageNumber * this.limit;
      this.search();
    }
  }

  search() {
    this.isSearch = true;

    this.requests = [];

    const filter = {
      limit: this.limit,
      offset: this.offset,
      adminSearch: this.adminSearch,
      searchString: this.searchString,
      status: this.statusId
    }

    this.requestsService.getRequests(filter).subscribe(res => {
      this.requests = res.result;
      this.pageCount = Math.ceil(res.found / this.limit);

      this.isSearch = false;
    });
  }

  goToCard(requestId: number) {
    if (this.goToCardEvent) {
      this.goToCardEvent.emit(requestId);
    }
  }

}
