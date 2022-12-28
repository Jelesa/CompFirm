import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RequestsService } from '../../../shared/requests.service';
import { RequestCard } from '../../models/request-card.model';

@Component({
  selector: 'app-cancel-request',
  templateUrl: './cancel-request.component.html',
  styleUrls: ['./cancel-request.component.css']
})
export class CancelRequestComponent implements OnInit {

  requestCard: RequestCard;

  confirmClicked: boolean = false;
  deleted: boolean = false;
  error: boolean = false;

  constructor(private requestService: RequestsService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit(): void {

    this.activatedRoute.data.subscribe(({ request }) => {
      this.requestCard = request;
    });
  }

  getNameGroup(): string {
    return `Заказ №${this.requestCard.number} от ${this.getUtcDate(this.requestCard.creationDate)}`.trim().replace('  ', ' ');
  }

  getUtcDate(dateString: string) {
    let date = new Date(dateString);

    return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()}`;
  }

  confirm() {

    this.confirmClicked = true;
    this.error = false;

    this.requestService.cancelRequest(+this.requestCard.number)
      .subscribe(_ => {
        this.deleted = true;

        setTimeout(_ => {
          this.router.navigate(['/cabinet']);
        }, 1000)

      }, error => {
        console.error(error);
        this.confirmClicked = false;
        this.error = true;
      });
  }

  cancel() {
    this.router.navigate([`/cabinet/request-card/${this.requestCard.number}`]);
  }
}
