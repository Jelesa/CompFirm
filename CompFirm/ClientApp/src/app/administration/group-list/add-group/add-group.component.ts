import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Group } from '../../../home/models/group.model';
import { GroupsService } from '../../../shared/groups.service';

@Component({
  selector: 'app-add-group',
  templateUrl: './add-group.component.html',
  styleUrls: ['./add-group.component.scss']
})
export class AddGroupComponent implements OnInit {

  defaultGroupItem: Group = {
    id: -1,
    name: '---'
  };

  error: boolean = false;
  edited: boolean = false;
  groupId: number;
  groups: Group[];

  formGroup: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private groupsServices: GroupsService) {
  }

  ngOnInit() {
    this.createForm();

    this.groupId = this.activatedRoute.snapshot.params['id'];

    this.groupsServices.getMainGroups().subscribe(g => {
      this.groups = [this.defaultGroupItem, ...g];
    });

  }

  cancel() {
    this.router.navigate([`administration/group-list`]);
  }

  confirm() {
    this.groupsServices.createGroup(this.formGroup.value)
      .subscribe(res => {
        this.edited = true;

        setTimeout(_ => {
          this.router.navigate([`administration/group-list`]);
        }, 1500)

      }, error => {
        this.error = true;
      });
  }

  private createForm() {
    this.formGroup = this.fb.group({
      parentGroupId: -1,
      name: ''
    });
  }

}
