import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TestBedComponent } from './TestBed/testbed.component';

const routes: Routes = [
  { path: 'home', component: TestBedComponent, data: { name: 'Home' } },
  { path: 'about', component: TestBedComponent, data: { name: 'About' } },
  { path: '', component: TestBedComponent },
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
