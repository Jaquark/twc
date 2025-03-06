import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';
import { RouterModule } from '@angular/router'; 
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [RouterModule, CommonModule], 
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Library Management System';
  isLoggedIn = false;
  userRole: string | null = null;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.isLoggedIn = this.authService.isAuthenticated();
    this.userRole = this.authService.getRole();
  }

  logout() {
    this.authService.logout();
    this.isLoggedIn = false;
    this.userRole = null;
  }
}
