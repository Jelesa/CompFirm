import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RequestCard } from '../../shared/models/request-card.model';
import { RequestsService } from '../../shared/requests.service';

@Component({
  selector: 'app-request-card',
  templateUrl: './request-card.component.html',
  styleUrls: ['./request-card.component.scss']
})
export class RequestCardComponent implements OnInit {

  isLoading: boolean = true;
  requestCard: RequestCard;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private requestService: RequestsService) { }

  ngOnInit() {
    this.requestService
    this.activatedRoute.data.subscribe(({ request }) => {
      this.requestCard = request;
      this.isLoading = false;
    });
  }

  private getRequestCard

  getUtcDate(dateString: string) {
    let date = new Date(dateString);

    return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()}`;
  }

  getUtcDateTime(dateString: string) {
    let date = new Date(dateString);

    return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}`;
  }

  moveToList() {
    this.router.navigate(['/cabinet']);
  }

  cancelRequest() {
    if (confirm('Вы действительно хотите отменить данный заказ?')) {
      this.isLoading = true;

      this.requestService.cancelRequest(+this.requestCard.number)
        .subscribe(_ => {
          this.isLoading = false;

          alert('Заказ отменен');
          this.router.navigate(['/cabinet']);

        }, error => {
          console.error(error);
          alert('Произошла ошибка при отмене заказа. Повторите попытку позже');
        });
    }
  }

}
