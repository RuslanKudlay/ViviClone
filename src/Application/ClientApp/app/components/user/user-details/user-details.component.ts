import { UserStatus } from './../../../models/user-model';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DeliveryServiceModel } from '../../../models/delivery-service.model';
import { OrderModel } from '../../../models/order.model';
import { UserService } from '../../../services/user.service';
import { UserModel } from '../../../models/user-model';

@Component({
    templateUrl: './user-details.component.html'
})
export class UserDetailsComponent implements OnInit {

    public loading: boolean = false;
    public user: UserModel = new UserModel();
    public roles: {id, name}[];
    public order: OrderModel;
    public deliveryServices: DeliveryServiceModel[];
    public selectedRole: {id, name};
    private userId: number;

    public selectedStatus: UserStatus;
    public UserStatus = UserStatus;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private userService: UserService,
    ) { }

    public ngOnInit(): void {
        this.loading = true;
        this.userService.getRoles().subscribe(response => {
            this.roles = response;
            this.route.params.subscribe(params => {
                this.userId = params['id'];
                this.userService.getUser(this.userId).subscribe(response => {
                    this.user = response;
                    this.selectedStatus = this.user.status;
                    this.selectedRole = this.roles.find(_ => _.name == this.user.role);
                    if (!this.selectedRole) {
                        this.selectedRole = this.roles[0];
                    }
                    this.loading = false;
                });
            });
        });
    }

    public changeRole(): void {
        console.log(this.selectedRole);
        this.userService.changeRole(this.selectedRole, this.userId).subscribe(_ => {
            this.router.navigate(['users']);
        });
    }

    public changeStatus(): void {
        this.userService.changeStatus(this.selectedStatus, this.userId).subscribe(_ => {
            this.router.navigate(['users']);
        });
    }
}
