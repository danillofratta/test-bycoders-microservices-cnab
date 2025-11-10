import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Observable, catchError, delay, finalize, map, of } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-sale',
  templateUrl: './list-sale.component.html',
  styleUrl: './list-sale.component.css'
})

export class ListSaleComponent {
 }
//export class ListSaleComponent implements OnInit, AfterViewInit {
 
//   length = 0;
//   pageSize = 10;
//   pageIndex = 0;

//   public list$: any[] = [];
    
//   public busy = false;

//   public _ListError: string[] = [];

//   dataSource = new MatTableDataSource<GetSaleResponseDto>();

//   displayedColumns = ['actions', 'id', 'number', 'customerName', 'branchName', 'status', 'totalAmount'];

//   @ViewChild(MatPaginator) paginator: any = MatPaginator;
  
//   constructor(private Api: SaleApi, private router: Router, private signal: SaleApiSignalRSevice) {
//   }

//   ngAfterViewInit(): void {
//     this.dataSource.paginator = this.paginator;
//   }

//   async ngOnInit() {
//     this.busy = true;

//     //this.signal.onGetListUpdated((updatedDataList: GetSaleResponseDto[]) => {
//     //  this.dataSource.data = updatedDataList;
//     //  console.log('SignalR updated data:', updatedDataList);
//     //  //this.cdr.detectChanges();
//     //});

//     await this.LoadList();
//   }

//   ClearErrorList() {
//     this._ListError = [];
//   }

//   onPageChange(event: PageEvent) {
//     this.busy = true;
//     this.pageIndex = event.pageIndex;
//     this.pageSize = event.pageSize;
//     this.LoadList();
//   }

//   async LoadList() {
//     this.busy = true;
    
//     this.ClearErrorList();

//     await new Promise(resolve => setTimeout(resolve, 2000));

//     (await this.Api.GetListAll(this.pageIndex + 1, this.pageSize)).pipe(
//       catchError((error) => {        
//           this._ListError.push(error.message || 'Erro ao carregar dados');
//           return of(null); // Retorna null em caso de erro
//         }),
//         finalize(() => (this.busy = false))
//       )
//       .subscribe((response) => {
//         if (response && response.success) {
//           this.dataSource.data = response.data.data;
//           this.length = response.data.totalCount;
//           this.pageIndex = response.data.currentPage - 1; // Ajuste para índice base 0
//           this.paginator.pageIndex = this.pageIndex;
//           this.paginator.length = this.length;
//         } else if (response && !response.success) {
//           this._ListError.push(response.message || 'Erro na resposta da API');
//         }
//       });
//   }

//   async onCancel(id: string) {
//     this.busy = true;
//     this.ClearErrorList();
//     const record: CancelSaleResponseDto = { Id: id }

//     await (await this.Api.Cancel(record)).subscribe({
//       next: (response) => {
//         if (response.success) {
//           this.router.navigate(['/sale']);
//         } else {
//           this._ListError.push(response.message);
//         }
//       },
//       error: (error) => {
//         console.error('Error occurred:', error);
        
//         this._ListError.push(error.error.message);     
//       },
//       complete: () => {
        
//       }
//     });
    
//     this.LoadList();    
//   }

//   async onDelete(id: string) {
//     this.busy = true;
//     this.ClearErrorList();
//     const record: DeleteSaleResponseDto = { Id: id }

//     await (await this.Api.Delete(record)).subscribe({
//       next: (response) => {
//         if (response.success) {
//           this.router.navigate(['/sale']);
//         } else {
//           this._ListError.push(response.message);
//         }
//       },
//       error: (error) => {
//         console.error('Error occurred:', error);

//         this._ListError.push(error.error.message);
//       },
//       complete: () => {

//       }
//     });

//     this.LoadList();
//   }

//   async onUpdate(id: string) {
//     this.router.navigate(['/sale/modify/' + id]);    
//   }
// }

//}
