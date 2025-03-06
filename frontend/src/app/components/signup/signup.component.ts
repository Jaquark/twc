import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';  // Example service for HTTP requests
import { ReactiveFormsModule } from '@angular/forms';  

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
  imports: [ReactiveFormsModule],
})
export class SignupComponent implements OnInit {
  signupForm: FormGroup;

  constructor(private fb: FormBuilder, private apiService: UserService) {
    this.signupForm = this.fb.group({
        username: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        role: ['Customer', Validators.required]  // Default to "Customer"
      });
  }

  ngOnInit(): void { }

  onSubmit(): void {
    if (this.signupForm.valid) {
      this.apiService.signup(this.signupForm.value).subscribe(
        (response) => {
          console.log('Signup successful', response);
          // Handle success, redirect to login or home page
        },
        (error) => {
          console.error('Error during signup', error);
          // Handle error
        }
      );
    }
  }
}