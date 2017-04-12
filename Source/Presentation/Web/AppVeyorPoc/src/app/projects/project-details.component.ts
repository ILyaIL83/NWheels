﻿import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Project } from './project';
import { AppVeyorService } from '../app-veyor.service';

@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html'
})

export class ProjectDetailsComponent implements OnInit {

  private sub: any;
  project: Project;

  constructor(private route: ActivatedRoute, private appVeyorService: AppVeyorService) { }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(params =>
      this.appVeyorService.getProjectByName(params['projectName']).then(project => this.project = project));
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}