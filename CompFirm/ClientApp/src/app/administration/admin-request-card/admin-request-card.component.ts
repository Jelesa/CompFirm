import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { REQUEST_STATUSES } from '../../shared/consts/requests-status-names.model';
import { RequestCard } from '../../shared/models/request-card.model';
import { RequestsService } from '../../shared/requests.service';

@Component({
  selector: 'app-admin-request-card',
  templateUrl: './admin-request-card.component.html',
  styleUrls: ['./admin-request-card.component.scss']
})
export class AdminRequestCardComponent implements OnInit {

  private requestId: number;

  isLoading: boolean = true;
  requestCard: RequestCard;

  statusEnum: typeof REQUEST_STATUSES = REQUEST_STATUSES;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private requestService: RequestsService) { }

  ngOnInit() {
    this.requestId = this.activatedRoute.snapshot.params['id'];
    this.setRequestCard();
  }

  private setRequestCard() {
    this.isLoading = true;

    this.requestService.getRequestCard(this.requestId).subscribe(card => {
      this.isLoading = false;
      this.requestCard = card;
    })
  }

  getUtcDate(dateString: string) {
    let date = new Date(dateString);

    return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()}`;
  }

  getUtcDateTime(dateString: string) {
    let date = new Date(dateString);

    return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}`;
  }

  moveToList() {
    this.router.navigate([`administration/request-list`]);
  }

  statusUpdate(statusName: string) {
    this.requestService.statusUpdate(this.requestCard.number, statusName)
      .subscribe(_ => {
        this.setRequestCard();
      });
  }

}
