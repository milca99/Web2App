import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserRole } from 'src/app/shared/enums/user-role.enum';
import { RegistrationUser } from 'src/app/shared/models/user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  alertify: any;
  title = 'google-places-autocomplete';
  registerationForm!: FormGroup ;
  selectedFile!: File;
  user!: RegistrationUser;
  userRole!: string;
  id: any;

  handleAddressChange(address: any) {
  }

  constructor(private router: Router,
              private fb: FormBuilder,
              private authService: AuthenticationService,
              private toastr: ToastrService,
              private userService: UserService
              ) {

      this.createRegisterationForm();
  }

  ngOnInit() {

  }

  createRegisterationForm() {
    this.registerationForm = this.fb.group({
      username: ["primerko",Validators.required],
      email: ["primerkoica@gmail.com",[Validators.required,Validators.email]],
      firstname: ["Primerko",[Validators.required,Validators.minLength(3)]],
      lastname: ["Prezimenko",[Validators.required,Validators.minLength(3)]],
      birthday: ["2001-05-05",[Validators.required]],
      address: ["Gagarinova",[Validators.required]],
      password: ["password", [Validators.required, Validators.minLength(8)]],
      confirmPassword: ["password", Validators.required],
    },{validators: this.passwordMatchingValidatior});

  }

  passwordMatchingValidatior(fg: FormGroup): Validators {
      return fg.get('password')?.value === fg.get('confirmPassword')?.value ? true : {notmatched: true};
  }

  
  onSubmitUser() {
    this.userRole = "Customer"
    this.OnSubmit();
  }

  onSubmitDeliverer() {
    this.userRole = "Seller"
    this.OnSubmit();
  }

  OnSubmit(){
    if (this.registerationForm.valid) { 
      if(this.selectedFile){
        console.log(this.userData())
        this.authService.register(this.userData()).subscribe(
          data=>{
            this.toastr.success('You have registered correctly, try loging in now', 'Succes!', {
              timeOut: 3000,
              closeButton: true,
            });
              
            this.router.navigate(['/authentication/login']);
          }, error =>{
            this.toastr.error("Invalid input", 'Error!' , {
              timeOut: 3000,
              closeButton: true,
            });
          }

        );
      }
      else{
        this.toastr.error("Please select profile picture", 'Error!' , {
          timeOut: 3000,
          closeButton: true,
        });
      }
    }
    else{
      this.toastr.error("You have to input every field valid", 'Error!' , {
        timeOut: 3000,
        closeButton: true,
      });
    }
  }

  userData(): RegistrationUser {
    return this.user = {
        username: this.username.value,
        email: this.email.value,
        firstName: this.firstname.value,
        lastName: this.lastname.value,
        birthday: this.registerationForm.value['birthday'],
        address: this.address.value,
        role: this.userRole,
        password: this.password.value,
        picture: "../../../assets/pictures/" + this.selectedFile.name
    };
  }

  onFileChanged(imageInput: any){
    this.selectedFile = imageInput.files[0];
    console.log(this.selectedFile);
  }

  get username() {
    return this.registerationForm.get('username') as FormControl;
  }
  get email() {
    return this.registerationForm.get('email') as FormControl;
  }
  get firstname() {
    return this.registerationForm.get('firstname') as FormControl;
  }
  get lastname() {
    return this.registerationForm.get('lastname') as FormControl;
  }
  get address() {
    return this.registerationForm.get('address') as FormControl;
  }
  get password() {
      return this.registerationForm.get('password') as FormControl;
  }
  get confirmPassword() {
      return this.registerationForm.get('confirmPassword') as FormControl;
  }

}
