import { Component, OnInit } from '@angular/core';
import { CharacteristicsService } from "../../shared/characteristics.service";
import { Characteristic } from "../../shared/models/characteristics.model";

@Component({
  selector: 'app-characteristics-list',
  templateUrl: './characteristics-list.component.html',
  styleUrls: ['./characteristics-list.component.scss']
})
export class CharacteristicsListComponent implements OnInit {
  isSearch: boolean = false;
  adding: boolean = false;
  addingName: string = '';
  addingUnit: string = '';

  characteristics: Characteristic[];

  constructor(private characteristicsService: CharacteristicsService) { }

  ngOnInit() {
    this.search();
  }

  search() {
    this.characteristicsService.getAll().subscribe(x => {
      this.characteristics = x;
    });
  }

  add() {
    this.adding = true;
  }

  undoAdding() {
    this.adding = false;
  }

  acceptAdding() {
    this.characteristicsService.create(this.addingName, this.addingUnit).subscribe(res => {

      alert('Успешно добавлено');
      this.adding = false;
      this.addingName = '';
      this.addingUnit = '';
      this.search();

    }, error => {
    });
  }

  delete(id: number) {
    if (confirm('Вы действительно хотите удалить?')) {

      this.characteristicsService.delete(id)
        .subscribe(_ => {

          alert('Успешно');
          this.search();

        }, error => {
        });
    }
  }
}
