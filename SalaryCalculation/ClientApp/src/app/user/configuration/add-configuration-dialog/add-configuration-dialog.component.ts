import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material";

@Component({
  selector: 'app-add-configuration-dialog',
  templateUrl: './add-configuration-dialog.component.html',
  styleUrls: ['./add-configuration-dialog.component.css'],
})
export class AddConfigurationDialogComponent implements OnInit {

  configForm: FormGroup;

  constructor(public dialogRef: MatDialogRef<AddConfigurationDialogComponent>,
              private fb: FormBuilder) {
  }

  ngOnInit() {
    this.configForm = this.fb.group({
      code: ['', [Validators.required]],
      configValue: ['', []],
      description: ['', []],
    });
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit() {
    this.dialogRef.close(this.configForm.value);
  }

}
