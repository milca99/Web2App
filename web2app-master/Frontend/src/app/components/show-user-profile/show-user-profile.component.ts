import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserDetails } from 'src/app/shared/models/user';
import { AuthService } from 'src/app/shared/services/auth.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-show-user-profile',
  templateUrl: './show-user-profile.component.html',
  styleUrls: ['./show-user-profile.component.css']
})
export class ShowUserProfileComponent implements OnInit {
  user!: UserDetails;
  id: any;
  token: any;

  constructor(private route: Router,
              private userService: UserService,
              private authService: AuthService) { }

  ngOnInit() {
    this.getUserDetails();
  }

  getUserDetails(){
    this.token = localStorage.getItem('token');
    this.id = this.authService.getUserId(this.token);
    this.userService.getUserDetails(this.id).subscribe(
      data=>{
        this.user = data as UserDetails;
      }, error =>{
        console.log('Error occurred at show-user-profile.component.ts')
      }

    );
  }

  ChangeProfile(): void {
    this.route.navigateByUrl('home/profile');
  }
}
