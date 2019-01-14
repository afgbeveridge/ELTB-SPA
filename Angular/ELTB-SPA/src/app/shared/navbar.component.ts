
import { Component, Input } from "@angular/core";
import { Router, Route } from '@angular/router';

@Component({
  selector: "navbar",
  template: `
<nav class="navbar navbar-inverse navbar-fixed-top navbar-expand-lg">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggler" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a asp-controller="Home" asp-action="Index" class="navbar-brand">ELTB</a>
            </div>
            <div class="navbar-collapse collapse">
                <ul class="navbar-nav">
                    <li *ngFor="let route of routes" class="nav-item">
                        <a [routerLinkActive]="['active']" [routerLink]="[route.path]" class="nav-link">{{route.data.name}}</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    `
})

export class NavbarComponent {

  routes: Route[];

  constructor(router: Router) {
    console.log('Doing routes' + router.config.length);
    this.routes = router.config.filter(r => r.path && r.path.indexOf('*') < 0);
  }

}
