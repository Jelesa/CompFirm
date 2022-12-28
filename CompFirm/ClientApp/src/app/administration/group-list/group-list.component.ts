import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Group } from '../../home/models/group.model';
import { GroupsService } from '../../shared/groups.service';

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.scss']
})
export class GroupListComponent implements OnInit {
  groups: Group[];

  isSearch: boolean = false;
  pageCount: number = 0;
  pageNumber: number = 0;
  limit: number = 10;
  offset: number = 0;

  constructor(
    private router: Router,
    private groupsServices: GroupsService) { }

  ngOnInit() {
    this.search();
  }

  search() {
    this.isSearch = true;

    this.groups = [];

    this.groupsServices.getGroups(this.limit, this.offset).subscribe(g => {
      this.groups = g.result;
      this.pageCount = Math.ceil(g.found / this.limit);

      this.isSearch = false;
    })
  }

  editGroup(id: number) {
    this.router.navigate([`administration/edit-group/${id}`]);
  }

  deleteGroup(id: number) {
    if (confirm('Вы действительно хотите удалить данную категорию?')) {

      this.groupsServices.deleteGroup(id)
        .subscribe(_ => {

          alert('Категория удалена');
          this.offset = 0;
          this.search();

        }, error => {
          console.error(error);
          alert('Произошла ошибка при удалении категории. Повторите попытку позже');
        });
    }
  }

  prevPage() {
    if (this.pageNumber > 0) {
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

  moveToAdd() {
    this.router.navigate(['administration/create-group'])
  }

}
