import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router'; 



import { LoginComponent } from './components/login/login.component';
import { BookSearchComponent } from './components/book-search/book-search.component';
import { MyCheckoutsComponent } from './components/my-checkouts/my-checkouts.component';
import { BookListComponent } from './components/book-list/book-list.component';
import { AddBookComponent } from './components/add-book/add-book.component';
import { BookDetailsComponent } from './components/book-details/book-details.component';
import { EditBookComponent } from './components/edit-book/edit-book.component';
import { CommonModule } from '@angular/common';

// Import Guards & Services
import { AuthGuard } from './guards/auth.guard';
import { AuthService } from './services/auth.service';

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    AppComponent,
    LoginComponent,
    BookSearchComponent,
    MyCheckoutsComponent,
    BookListComponent,
    AddBookComponent,
    BookDetailsComponent,
    EditBookComponent,
    CommonModule
  ],
  providers: [AuthGuard, AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
