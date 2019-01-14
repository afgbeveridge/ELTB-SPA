import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NavbarComponent } from "./shared/navbar.component";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';

import { TestBedComponent } from './TestBed/testbed.component';
import { LanguageComponent } from './TestBed/language.component';
import { ExecutionComponent } from './TestBed/execution.component';
import { ExpandableSummaryComponent } from './shared/expandable-summary.component';
import { SummariserPipe } from './shared/summariser.pipe';

import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent, NavbarComponent, TestBedComponent, LanguageComponent, ExecutionComponent, ExpandableSummaryComponent, SummariserPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [ ],
  bootstrap: [AppComponent]
})
export class AppModule { }
