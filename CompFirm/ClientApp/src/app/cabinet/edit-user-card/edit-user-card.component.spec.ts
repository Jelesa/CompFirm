import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { EditUserCardComponent } from './edit-user-card.component';


describe('EditUserCardComponent', () => {
  let component: EditUserCardComponent;
  let fixture: ComponentFixture<EditUserCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EditUserCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditUserCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
