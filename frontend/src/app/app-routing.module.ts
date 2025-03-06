import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { BookSearchComponent } from './components/book-search/book-search.component';
import { MyCheckoutsComponent } from './components/my-checkouts/my-checkouts.component';
import { BookListComponent } from './components/book-list/book-list.component';
import { AddBookComponent } from './components/add-book/add-book.component';
import { BookDetailsComponent } from './components/book-details/book-details.component';
import { EditBookComponent } from './components/edit-book/edit-book.component';
import { SignupComponent } from './components/signup/signup.component'; 
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent }, 
  { path: 'login', component: LoginComponent },
  { path: 'book-search', component: BookSearchComponent },
  { path: 'my-checkouts', component: MyCheckoutsComponent, canActivate: [AuthGuard] },
  { path: 'books', component: BookListComponent, canActivate: [AuthGuard] }, 
  { path: 'add-book', component: AddBookComponent, canActivate: [AuthGuard] },
  { path: 'book/:id', component: BookDetailsComponent },
  { path: 'edit-book/:id', component: EditBookComponent, canActivate: [AuthGuard] },
  { path: 'signup', component: SignupComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }


