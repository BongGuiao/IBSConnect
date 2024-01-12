import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MembersService } from "../../services/members.service";
import { ToastrService } from 'ngx-toastr';
import { ImportResult } from "../../models/models";

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.scss']
})
export class ImportComponent implements OnInit {
  isUploaded: boolean = false;

  @ViewChild('file')
  file: ElementRef;

  importResult!: ImportResult;

  constructor(
    private toastrService: ToastrService,
    private membersService: MembersService
  ) { }

  ngOnInit(): void {
  }

  uploadMembers() {
    let file = <HTMLInputElement>this.file.nativeElement;

    if (file.files.length === 0) {
      //this.noFileSelected = true;
      return;
    }

    let fileToUpload = file.files[0];

    const formData = new FormData();

    formData.append('file', fileToUpload, fileToUpload.name);
    this.isUploaded = false;

    this.membersService.uploadMembers(formData).subscribe((d) => {
      this.isUploaded = true;
      this.importResult = d;
      this.toastrService.success("Member Data uploaded");
    });
  }

}
