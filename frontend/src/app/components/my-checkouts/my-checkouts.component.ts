import { Component, OnInit } from '@angular/core';
import { CheckoutService } from '../../services/checkout.service';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Checkout } from 'src/app/models/checkout.model';

@Component({
  selector: 'app-my-checkouts',
  templateUrl: './my-checkouts.component.html',
  styleUrls: ['./my-checkouts.component.css'],
  imports: [FormsModule, CommonModule], 
})
export class MyCheckoutsComponent implements OnInit {
  checkouts: Checkout[] = [];

  constructor(private checkoutService: CheckoutService, public authService: AuthService) {}

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.loadCheckouts();
    }
  }

  loadCheckouts(): void {
    this.checkoutService.getUserCheckouts().subscribe({
      next: data => this.checkouts = data,
      error: err => console.error('Failed to load checkouts', err)
    });
  }
}