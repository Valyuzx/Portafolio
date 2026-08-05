import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatSidenavContainer, MatSidenav, MatSidenavContent } from '@angular/material/sidenav';
import { BreakpointObserver } from '@angular/cdk/layout';
import { map } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { Sidebar, MenuItem } from '../../shared/components/sidebar/sidebar';
import { Topbar } from '../../shared/components/topbar/topbar';

@Component({
  selector: 'app-home',
  imports: [
    RouterOutlet,
    MatSidenavContainer,
    MatSidenav,
    MatSidenavContent,
    Sidebar,
    Topbar,
  ],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {
  private readonly breakpointObserver = inject(BreakpointObserver);

  readonly isMobile = toSignal(
    this.breakpointObserver.observe('(max-width: 800px)').pipe(map(result => result.matches)),
    { initialValue: false }
  );

  readonly menuItems: MenuItem[] = [
    { icon: 'dashboard', label: 'Dashboard',   route: '/home' },
    { icon: 'work',      label: 'Proyectos',   route: '/home/projects' },
    { icon: 'category',  label: 'Categorías',  route: '/home/categories' },
    { icon: 'memory',    label: 'Tecnologías', route: '/home/technologies' },
  ];
}
