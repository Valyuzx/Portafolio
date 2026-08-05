import { Component, inject, OnInit } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { AuthService } from '../../../core/services/auth.service';
import { ProjectService } from '../../project/services/project.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatIcon],
  templateUrl: './dashboard.html',
 
})
export class Dashboard implements OnInit {
  private readonly authService = inject(AuthService);
  readonly projectService = inject(ProjectService);

  readonly user = this.authService.currentUser;

  ngOnInit() {
    // Cargamos los proyectos en background para las estadísticas
    this.projectService.loadAllProjects().subscribe();
  }
}
