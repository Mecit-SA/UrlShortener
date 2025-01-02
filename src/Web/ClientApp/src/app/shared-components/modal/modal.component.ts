import { Component, Input, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
})
export class ModalComponent implements OnInit, OnDestroy {

  @Input() modalContent?: TemplateRef<any>;

  modalRef?: BsModalRef;

  constructor(private modalService: BsModalService) { }

  ngOnInit(): void {
    this.openModal();
  }

  ngOnDestroy(): void {
    this.closeModal();
  }

  openModal(): void {
    if (this.modalContent) {
      // Open modal and prevent it from closing when clicking outside
      this.modalRef = this.modalService.show(this.modalContent, { ignoreBackdropClick: true });
    }
  }

  closeModal(): void {
    if (this.modalRef) {
      this.modalRef.hide();
      this.modalRef = undefined; // Clear the modalRef after hiding it
    }
  }
}
