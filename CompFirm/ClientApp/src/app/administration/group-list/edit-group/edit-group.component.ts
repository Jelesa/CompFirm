import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Group } from '../../../home/models/group.model';
import { GroupsService } from '../../../shared/groups.service';

@Component({
  selector: 'app-edit-group',
  templateUrl: './edit-group.component.html',
  styleUrls: ['./edit-group.component.scss']
})
export class EditGroupComponent implements OnInit {

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

    this.groupsServices.getGroupById(this.groupId)
      .subscribe(g => {
        console.log(g);

        this.formGroup.setValue({
          id: g.id,
          parentGroupId: g.parentGroupId,
          name: g.name
        });
      });

    this.groupsServices.getMainGroups().subscribe(g => {
      const filteredGroups = g.filter(x => x.id != this.groupId);

      this.groups = [ this.defaultGroupItem, ...filteredGroups ];
    });

  }

  cancel() {
    this.router.navigate([`administration/group-list`]);
  }

  confirm() {
    this.groupsServices.editGroup(this.formGroup.value)
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
      id: -1,
      parentGroupId: -1,
      name: ''
    });
  }

}
