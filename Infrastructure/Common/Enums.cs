using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
        public enum SmsType
        {

            [Display(Name = "رد مدارک")]
            RejectDocuments = 10,
            [Display(Name = "تایید مدارک")]
            ConfirmDocuments = 20,

            [Display(Name = "پرداخت موفق")]
            PaymentSuccess = 30,

            [Display(Name = "تایید شماره تلفن")]
            ConfirmPhoneNumber = 40,
            [Display(Name = "فراموشی رمز عبور")]
            ForgotPassword = 50,
            [Display(Name = "  ثبت نام")]
            RegisterUser = 60,
            [Display(Name = "ادمین")]
            Admin = 70,
            [Display(Name = " خبر ")]

            News = 80,

            [Display(Name = "مشکلات سامانه ")]

            Ticket = 90,

            [Display(Name = " تغییر وضعیت سفارش")]
            ChangeRequestStatus = 100,
            [Display(Name = " ورود کاربر")]
            LoginByUser = 110,
            [Display(Name = "  فعال سازی حساب کاربری")]
            SetActiveUser = 120,
        }
        public enum CartableType
        {
            [Display(Name = "Company Requests")]
            CompanyRequest = 10,
            [Display(Name = "Request Review")]
            RequestReview = 20,
            [Display(Name = "Expert Check Request")]
            ExpertCheckRequest = 30,
            [Display(Name = "Commercial Manager Check Request")]
            CommercialManagerCheckRequest = 40,
            [Display(Name = "Procurement Manager Check Request")]
            ProcurementManagerCheckRequest = 50,
            [Display(Name = "Procurement Manager Check MR")]
            ProcurementManagerCheckMR = 60,
            [Display(Name = "Commission Check MR")]
            CommissionCheckMR = 70,
            [Display(Name = "MidTerm Evaluation")]
            MidTermEvaluation = 80,
            [Display(Name = "Final Evaluation")]
            FinalEvaluation = 90,
            [Display(Name = "Warranty Evaluation")]
            WarrantyEvaluation = 100,
            [Display(Name = "Bidder Interest Company Answer")]
            BidderInterestCompanyAnswer = 110,
            [Display(Name = "Bidder Interest Send To Accept Commission")]
            BidderInterestSendToAccept = 120,
            [Display(Name = "Bidder Interest Send To Invitation")]
            BidderInterestSendToInvitation = 130,
            [Display(Name = "Bidder Interest Invitation Company Answer")]
            BidderInterestInvitationCompanyAnswer = 140,
        }

        public enum BidderRequestType
        {
            [Display(Name = "Bidder Interest")]
            BidderInterest = 10,
            [Display(Name = "Bidder Accept")]
            BidderAccept = 20,
            [Display(Name = "Bidder Invitation")]
            BidderInvitation = 30,
        }
        public enum OrderStatus
        {
            [Display(Name = " ثبت درخواست و در انتظار پرداخت")]
            Register = 10,

            [Display(Name = "پرداخت موفق")]
            PaymentSuccess = 20,

            [Display(Name = "پرداخت ناموفق")]
            PaymentFailed = 30,

            [Display(Name = "در حال بررسی")]
            InProgress = 40,
            [Display(Name = "رد شده")]
            Reject = 50,
            [Display(Name = "آماده  برای ارسال")]
            Sending = 60,
            [Display(Name = "ارسال شده")]
            Sent = 70,
            [Display(Name = "تحویل داده شده")]
            Delivered = 80,
            [Display(Name = "مرجوع توسط مشتری")]
            ReturnByCustomer = 90,
        }

        public enum ShowStatus
        {
            [Display(Name = "  نمایش داده شود")]
            AllowVisit = 10,
            [Display(Name = "نمایش داده نشود")]
            NotAllowVisit = 20,

        }

        public enum PagesLinkType
        {
            [Display(Name = "  ارتباط با ما")]
            ContactUs = 10,
            [Display(Name = "دسترسی بیشتر")]
            MoreAccess = 20,

        }

        public enum UserOpinionType
        {
            [Display(Name = "  محصولات ")]
            Products = 1,
            [Display(Name = "  مقالات ")]
            Articles = 2,
            [Display(Name = "  خدمات ")]
            Services = 3,
        }

        public enum UseOpinionStatus
        {
            [Display(Name = "  Positive Opinion ")]
            PositiveOpinion = 10,
            [Display(Name = " Negative Opinion ")]
            NegativeOpinion = 20,

        }

        public enum UserMessageType
        {
            [Display(Name = "EIED User Register Confirmation")]
            RegisterConfirmation = 10,
            [Display(Name = "Reset Password Link")]
            ForgotPasswordConfirmation = 20,
            [Display(Name = "Send Company Request")]
            SendCompanyRequest = 30,
            [Display(Name = "Invalid Company Request Data")]
            InvalidCompanyRequestData = 40,
            [Display(Name = "Accept Company Request")]
            AcceptCompanyRequest = 50,
            [Display(Name = "Reject Company Request")]
            RejectCompanyRequest = 60,
            [Display(Name = "EIED Bidder Interest")]
            BidderInterest = 70,
            [Display(Name = "EIED Bidder Interest Invitation")]
            BidderInvitation = 71,
            [Display(Name = "EIED Bidder Interest Invitation Answer Reply")]
            BidderInterestInvitationAnswerReply = 80,
        }

        public enum InternalMessageType
        {
            [Display(Name = "Register New Company")]
            RegisterNewCompany = 10,
            [Display(Name = "Send New Company Request")]
            SendNewCompanyRequest = 20,
        }


        public enum QuestionEvalutionType
        {
            //[Display(Name = "Sourcing Inital Evalution")]
            [Display(Name = " Inital Evalution")]

            SourcingInitalEvalution = 10,
            //[Display(Name = "Purchase Order Mid Term Evalution")]
            [Display(Name = "Monthly Evalution")]

            PurchaseOrderMidTermEvalution = 20,

            //[Display(Name = "Purchase Order After Warranty Evalution")]
            [Display(Name = "Evalution After Gurantee Period ")]

            PurchaseOrderAfterWarrantyEvalution = 30,

            //[Display(Name = "Purchase Order Final Evalution")]
            [Display(Name = "Final Evalution")]

            PurchaseOrderFinalEvalution = 40,
        }

        public enum QuestionType
        {
            [Display(Name = "Text")]
            Text = 1,
            [Display(Name = "Number")]
            Number = 2,

        }

        public enum VideoSource
        {
            [Display(Name = "آپارات")]
            Aparat = 10,
            [Display(Name = "فایل محلی ")]
            LocalFile = 20,
            [Display(Name = "لینک مسقیم")]
            DirectLink = 30,

        }


        public enum PositionPlace
        {
            [Display(Name = "صفحه اول - قسمت اول - صفحه اصلی")]
            PartOne = 10,
            [Display(Name = "صفحه اول - قسمت دوم - صفحه اصلی")]
            PartTwo = 20,
            [Display(Name = " صفحه اول - قسمت سوم - صفحه اصلی ")]
            PartThree = 30,
            [Display(Name = " صفحه اول - قسمت چهارم - صفحه اصلی ")]
            PartFour = 40,
            [Display(Name = "پیشنهاد لحظه ای - صفحه جزئیات یک محصول ")]
            ImmediateOffer = 50,
            [Display(Name = "  بالای منو  ")]
            AboveTheMenu = 60,
            [Display(Name = "    پیشنهاد ویژه صفحه دسته بندی محصولات - در منوی محصولات - سطح اول  ")]
            OfferForProductCategory = 70,

            [Display(Name = "  پیشنهاد ویژه صفحه زیر گروه قطعه - در منوی محصولات - سطح دوم ")]
            OfferForPieceCategory = 90,

            [Display(Name = "  پیشنهاد ویژه صفحه  قطعه - برند - در منوی محصولات - سطح سوم  ")]
            OfferForPiece = 80,

            [Display(Name = "   پیشنهاد ویژه صفحه یک برند - در منوی برند ها - سطح اول  ")]
            OfferForBrand = 100,

            [Display(Name = "   پیشنهاد ویژه صفحه زیر شاخه برند - در منوی برند ها - سطح دوم   ")]
            OfferForBrandPiece = 110,


            [Display(Name = "   پیشنهاد ویژه صفحه زیر شاخه قطعه و برند - در منوی برند ها - سطح سوم  ")]
            OfferForBrandProductCategory = 120,

            [Display(Name = "   صفحه جزئیات محصول  ")]
            ProductDetails = 130,


            [Display(Name = "   صفحه ویژگی محصولی  ")]
            ProductFeatures = 140,

            [Display(Name = "   صفحه ویژگی برندی  ")]
            BrandFeatures = 150,
        }

        public enum Device
        {
            [Display(Name = " Mobile")]
            Mobile = 10,
            [Display(Name = " DeskTop")]
            DeskTop = 20,



        }

        public enum MRRegisterBy
        {
            [Display(Name = "Operator")]
            Operator = 10,
            [Display(Name = "Manager")]
            Manager = 20,
            [Display(Name = "Commission")]
            Commission = 30,
        }
        public enum InvitionAvlRegisterBy
        {
            [Display(Name = "Operator")]
            Operator = 10,
            [Display(Name = "Manager")]
            Manager = 20,
            [Display(Name = "Commission")]
            Commission = 30,
        }

        public enum RegisterBy
        {
            [Display(Name = "Admin EIED")]
            AdminEIED = 1,
            [Display(Name = "Purchase Order ")]
            PurchaseOrder = 2,
            [Display(Name = "Manager")]
            Manager = 3,
            [Display(Name = "Company")]
            Company = 4,
            [Display(Name = "Material Requisition Commission")]
            MaterialRequisitionCommission = 5,
        }

        public enum EmailTemplateType
        {
            [Display(Name = "Footer Email Template")]
            FooterEmailTemplate = 1,
            [Display(Name = "Reset Password Email")]
            ResetPasswordEmail = 2,
            [Display(Name = "Bidder Invitation For MR")]
            BidderInvitationForMRTemplate = 10,
            [Display(Name = "Vendor Accept Request")]
            VendorAcceptRequestTemplate = 20,
            [Display(Name = "Supplier Accept Request")]
            SupplierAcceptRequestTemplate = 30,
            [Display(Name = "Service Provider Accept Request")]
            ServiceProviderAcceptRequestTemplate = 40,
            [Display(Name = "Send Company Request")]
            SendCompanyRequest = 50,
            [Display(Name = "Register Confirmation Email")]
            RegisterConfirmationEmail = 60,
            [Display(Name = "Bidder Interest")]
            BidderInterest = 70,
            [Display(Name = "Bidder Invitation")]
            BidderInvitation = 80,
        }
        public enum VendorAttachmentType
        {
            [Display(Name = "Gazette Attachment")]
            GazetteAttachment = 1,
            [Display(Name = "Catalogue")]
            Catalogue = 2,
            [Display(Name = "UnderLicensed Agreement")]

            UnderLicensedAgreement = 3,
            [Display(Name = "Machinery And Production Facilities Attachment")]

            MachineryAndProductionFacilitiesAttachment = 4,
            [Display(Name = "Certificates")]

            Certificates = 5,
            [Display(Name = "Iranian Supplied Products Or Services List Attachment")]

            IranianSuppliedProductsOrServicesListAttachment = 6,
            [Display(Name = "Supplied Products OrServices To Other Countries")]

            SuppliedProductsOrServicesToOtherCountries = 7,
            [Display(Name = "All Current Projects Attachment")]

            AllCurrentProjectsAttachment = 8,
            [Display(Name = "Authorization Letters")]

            AuthorizationLetters = 9,
        }

        public enum SupplierAttachmentType
        {
            [Display(Name = "Authorization Letter")]
            AuthorizationLetter = 1,
            [Display(Name = "Catalogue")]
            Catalogue = 2,
            [Display(Name = "Performance Report Acceptance From Customers")]
            PerformanceReportAcceptanceFromCustomers = 3,
            [Display(Name = "Relevant Equipment List Attachment")]
            RelevantEquipmentListAttachment = 4,
            [Display(Name = "Iran Current Projects Attachment")]
            IranCurrentProjectsAttachment = 5,
            [Display(Name = "Other Current Projects Attachment")]
            OtherCurrentProjectsAttachment = 6,
        }




        public enum CompanyAttachmentType
        {
            Logo = 1,
            CV = 2
        }



        public enum ProductAttachmentType
        {
            [Display(Name = "Orginal")]
            Orginal = 1,
            [Display(Name = "Other")]
            Other = 2,
            [Display(Name = "Catalogue And Brochure")]
            CatalogueAndBrochure = 3,
        }


        public enum ProductStatusType
        {
            [Display(Name = "Register")]
            Register = 1,
            [Display(Name = "InProgress")]
            InProgress = 2,
            [Display(Name = " Final Approved")]
            Approved = 3,
            [Display(Name = "Final Reject")]
            Reject = 4,

        }

        public enum FormDataStatusType
        {
            [Display(Name = "NotModify")]
            NotModify = 1,
            [Display(Name = "Added")]
            Added = 2,
            [Display(Name = "Edited")]
            Edited = 3,
            [Display(Name = "Deleted")]
            Deleted = 4,

        }



        public enum FileType
        {
            [Display(Name = "Image")]
            Image = 1,
            [Display(Name = "Pdf")]
            Pdf = 2,
            [Display(Name = "Word")]
            Word = 3,
            [Display(Name = "Excel")]
            Excel = 4,
            [Display(Name = "ZIP")]
            ZIP = 5,
            [Display(Name = "RAR")]
            RAR = 4,
            [Display(Name = "MP4")]
            MP4 = 5,
            [Display(Name = "OGG")]
            OGG = 6,
        }


        public enum CompanyRequestStatus
        {
            [Display(Name = "Initial InProgress")]
            InProgress = 1,
            [Display(Name = "Initial Accept ")]
            Accept = 2,
            [Display(Name = "Initial Reject")]
            Reject = 3,
        }

        public enum EvaluationStep
        {
            [Display(Name = "Initial Data Evaluation")]
            InitialDataEvaluation = 1,
            [Display(Name = "Pre-Qualification Document")]
            VirtualEvaluationByTheEvaluationTeam = 2,
            [Display(Name = "Shop Visit")]
            PhysicalAssessmentByTheAssessmentTeam = 3,
        }

        public enum CompanyRequestWorkFlowStatus
        {
            [Display(Name = "Send Request With Company")]
            SendRequestWithCompany = 10,
            [Display(Name = "Check Company Data")]
            CheckCompanyData = 20,
            [Display(Name = "Accept Company Data")]
            AcceptCompanyData = 30,
            [Display(Name = "Reject Company Data")]
            RejectCompanyData = 40,
            [Display(Name = "Check Request With Commision")]
            CheckRequestWithCommision = 50,

            [Display(Name = " Return Request With Expert")]
            ReturnRequestWithExpert = 140,


            [Display(Name = "Evaluate ( Pre-Qualification Document )")]

            VirtualInternalExpertGroup = 60,

            [Display(Name = "Evaluate ( Shop Visit )")]

            PhysicalExpertGroup = 70,

            [Display(Name = "Check Request With Commercial Manager")]
            CheckRequestWithCommercialManager = 80,
            [Display(Name = " Return Request With Commercial Manager")]
            ReturnRequestWithCommercialManager = 150,
            [Display(Name = "Accept Request With Commercial Manager")]
            AcceptRequestWithCommercialManager = 90,
            [Display(Name = "Reject Request With Commercial Manager")]
            RejectRequestWithCommercialManager = 100,


            [Display(Name = "Check Request With Procurement Manager")]
            CheckRequestWithManager = 110,

            [Display(Name = "Return Request With Procurement Manager")]
            ReturnRequestWithManager = 160,

            [Display(Name = "Accept Request With Procurement Manager")]
            AcceptRequestWithManager = 120,
            [Display(Name = "Reject Request With Procurement Manager")]
            RejectRequestWithManager = 130,




        }

        public enum MRCheckStatus
        {
            [Display(Name = "Not Checked")]
            NotChecked = 1,
            [Display(Name = " Manger Checked")]
            MangerChecked = 2,
            [Display(Name = "Commission Checked ")]
            CommissionChecked = 3,
            [Display(Name = "Checked All")]
            CheckedAll = 4,
        }

        public enum CompanyQuestionStatus
        {
            [Display(Name = "NotVisited")]
            NotVisited = 1,
            [Display(Name = "Visited")]
            Visited = 2,
            [Display(Name = "Answered")]
            Answered = 3,
        }

        public enum BiddInterestConfirmType
        {
            [Display(Name = "Interested")]
            Interested = 1,
            [Display(Name = "Not Interested")]
            NotInterested = 2,
            [Display(Name = "Not Answered")]
            NotAnswered = 3,
            [Display(Name = "Not Visited")]
            NotVisited = 4,
            [Display(Name = "Visited")]
            Visited = 5,
        }

        public enum ReplyEmailStatus
        {
            [Display(Name = "Answered")]
            Answered = 1,
            [Display(Name = "Not Answered")]
            NotAnswered = 2,

        }



        public enum SendEmailType
        {
            [Display(Name = "All")]
            All = 1,
            [Display(Name = "Selected")]
            Selected = 2,
            [Display(Name = "OneItem")]
            OneItem = 3,
            [Display(Name = "NotAnswer")]
            NotAnswer = 4,
            [Display(Name = "OneItemForAnswer")]
            OneItemForAnswer = 5,
        }


        public enum CompanyType
        {
            [Display(Name = "Manufacture")]
            Vendor = 1,
            [Display(Name = "Supplier")]
            Supplier = 2,
            [Display(Name = "ServiceProvider")]
            ServiceProvider = 3,
        }



        public enum CompanyStatus
        {
            [Display(Name = "All")]

            All = 1,
            [Display(Name = "Avl Approved")]

            AvlApproved = 2,
            [Display(Name = "Avl Not Approved")]

            AvlNotApproved = 3,
        }

        public enum EvaluteType
        {
            [Display(Name = "Evaluted")]
            Evaluted = 1,
            [Display(Name = "NotEvaluted")]
            NotEvaluted = 2,

        }


        public enum BidderRequestAction
        {
            [Display(Name = "Material Requisition Add")]

            MaterialRequisitionAdd = 1,
            [Display(Name = "Material Requisition Delete")]

            MaterialRequisitionDelete = 2,


            [Display(Name = "MRCheck With Manager Add")]

            MRCheckWithManagerAdd = 3,

            [Display(Name = "MRCheck With Manager Add")]

            MRCheckWithManagerDelete = 4,

            [Display(Name = "MRCheck With Commission Add")]

            MRCheckWithCommissionAdd = 5,
            [Display(Name = "MRCheck With Commission Delete")]

            MRCheckWithCommissionDelete = 6,

            [Display(Name = "Material Requisition Update")]

            MaterialRequisitionUpdate = 7,

            [Display(Name = "MRCheck With Manager Delete")]

            MRCheckWithManagerUpdate = 8,

            [Display(Name = "MRCheck With Commission  Delete")]

            MRCheckWithCommissionUpdate = 9,



        }

        public enum TypeAction
        {
            [Display(Name = "Add")]
            Add = 1,
            [Display(Name = "Edit")]
            Edit = 2,
            [Display(Name = "Delete")]
            Delete = 3,
        }


        public enum SaleStatus
        {
            [Display(Name = "در حال فروش")]
            InProgressSelling = 1,
            [Display(Name = "توقف فروش")]
            StopSelling = 2,

        }


        public enum EquipmentType
        {
            [Display(Name = "ماشین آلات راه سازی")]
            A = 1,
            [Display(Name = "امکانات ترخیص")]
            B = 2,
            [Display(Name = "تجهیزات حمل و نقل")]
            C = 3,
        }


        public enum PersonnelPosition
        {
            [Display(Name = "مدیرعامل")]
            CEO = 1,
            [Display(Name = "مدیر فنی")]
            CTO = 2,
        }

        public enum Sex
        {
            [Display(Name = "مرد")]
            Male = 1,
            [Display(Name = "زن")]
            Female = 2,
        }

        public enum AmountUnit
        {
            Default = 1,
            MilionRial = 2,
            MilyardRial = 3,
        }

        public enum ActionType
        {
            GET = 1,
            POST = 2,
            PUT = 3,
            DELETE = 4,
            GETPOST = 5,
        }

        public enum FilterType
        {
            Text = 1,
            Combo = 2,
            Date = 3,
            Tree = 4,
            Checkbox = 5,
        }

        public enum DataSourceType
        {
            Sp = 1,
            Query = 2,

        }
        public enum FormatColumns
        {
            CommaSeparator = 1,
            ShamsiDate = 2,
        }

        public enum TrialBalanceLevelDetail
        {
            Level01 = 1,
            Level02 = 2,
            Level03 = 3,
            Level04 = 4,

        }

        /// <summary>
        /// نمایش روند
        /// </summary>
        public enum ShowTrends
        {
            [Display(Name = "Daily")]
            Daily = 101,

            //[Display(Name = "هفتگی")]
            //Weekly = 102,

            [Display(Name = "Monthly")]
            Monthly = 103,

            [Display(Name = "Three Month")]
            ThreeMonths = 104,

            [Display(Name = "Six Month")]
            SixMonths = 105,

            [Display(Name = "Yearly")]
            Yearly = 106,
        }

        public enum DurationUnit
        {
            [Display(Name = "Daily")]
            Daily = 101,

            [Display(Name = "Weekly")]
            Weekly = 102,

            [Display(Name = "Monthly")]
            Monthly = 103,
        }

        /// <summary>
        ///  نحوه محاسبه
        /// </summary>
        public enum CalcType
        {
            [Display(Name = "Simple")]
            Normal = 1,
            [Display(Name = "Percent")]
            Percentage = 2,
        }



        /// <summary>
        ///   نوع تعداد
        /// </summary>
        public enum CountType
        {
            [Display(Name = "ثابت")]
            Static = 1,
            [Display(Name = "متغیر")]
            Variable = 2,
        }



        public enum DeliveryType
        {
            [Display(Name = "پست")]
            Post = 1,
            [Display(Name = "تیپاکس")]
            Tipax = 2,
            [Display(Name = "پیک موتوری")]
            BikeDelivery = 3,
        }




        public enum Province
        {
            [Display(Name = "آذربایجان شرقی")]

            Azerbaijansharghi = 1,

            [Display(Name = "آذربایجان غربی")]

            Azerbaijangharbi = 2,
            [Display(Name = "اردبیل")]

            Ardebil = 3,
            [Display(Name = "اصفهان")]

            Esfehan = 4,
            [Display(Name = "البرز")]

            Alborz = 5,
            [Display(Name = "ایلام")]

            Ilam = 6,
            [Display(Name = "بوشهر")]

            Bushehr = 7,
            [Display(Name = "تهران")]

            Tehran = 8,
            [Display(Name = "چهارمحال و بختیاری")]

            Chvab = 9,
            [Display(Name = "خراسان جنوبی")]

            Khorasanjunubi = 10,
            [Display(Name = "خراسان رضوی")]

            Khorasanrazavi = 11,
            [Display(Name = "خراسان شمالی")]

            Khorasanshomali = 12,

            [Display(Name = "خوزستان")]
            Khuzestan = 13,

            [Display(Name = "زنجان")]
            Zanjan = 14,

            [Display(Name = "سمنان")]
            Semnan = 15,

            [Display(Name = "سیستان و بلوچستان")]
            Sistan = 16,

            [Display(Name = "فارس")]
            Fars = 17,

            [Display(Name = "قزوین")]
            Ghazvin = 18,

            [Display(Name = "قم")]
            Ghom = 19,

            [Display(Name = "کردستان")]
            Kordestan = 20,

            [Display(Name = "کرمان")]
            Kerman = 21,

            [Display(Name = "کرمانشاه")]
            Kermanshah = 22,

            [Display(Name = "کهکیلویه و بویراحمد")]
            kvab = 23,

            [Display(Name = "گلستان")]
            Golestan = 24,


            [Display(Name = "گیلان")]
            Gilan = 25,

            [Display(Name = "لرستان")]
            Lorestan = 26,

            [Display(Name = "مازندران")]
            Mazandaran = 27,

            [Display(Name = "مرکزی")]
            Markazi = 28,

            [Display(Name = "هرمزگان")]
            Hormozgan = 29,

            [Display(Name = "همدان")]
            Hamedan = 30,

            [Display(Name = "یزد")]
            Yazd = 31,

        }

        public enum ControlType
        {
            [Display(Name = "Text")]
            Text = 101,
            [Display(Name = "Numeric")]
            Number = 102,
            [Display(Name = "True-False")]
            Switch = 103,
            [Display(Name = "Multi Option")]
            Tag = 104,
            [Display(Name = "List")]
            DropDown= 105,
        }

      
        public enum UnitType 
        {
            [Display(Name = "Percent")]
            Percentage = 101,

            [Display(Name = "Number")]
            Number = 102,

            [Display(Name = "Hours")]
            Hours = 103,

            [Display(Name = "Person")]
            Person = 104,

            [Display(Name = "PersonHour")]
            PersonHour = 105,

             [Display(Name = "نفر در دسته سابقه")]
             PersonPerGroupHistory = 106,

            [Display(Name = "Daily")]
            Daily = 107,

            [Display(Name = "Monthly")]
            Monthly = 116,

            [Display(Name = "Year")]
            Year = 108,

            [Display(Name = "Meter")]
            Meter = 109,

            [Display(Name = "CubicMeters")]
            MeterMokab = 110,

            [Display(Name = "SquareMeters")]
            SquareMeter = 111,

            [Display(Name = "Ton")]
            Ton = 112,

            [Display(Name = "Rial")]
            Rial = 113,

            [Display(Name = "Kg")]
            Kg = 114,

            [Display(Name = "Inch/Dia")]
            InchDia = 115,

            [Display(Name = "True-False")]
            TrueFalse = 117,
        }

      
        public enum ShowType
        {
            [Display(Name = "Table")]
            Grid = 101,

            [Display(Name = "Pie Chart")]
            Pie = 102,

            [Display(Name = "Line Chart")]
            Line = 103,

            [Display(Name = "Bar Chart")]
            Bar = 104,

            [Display(Name = "Gauge")]
            Gauge = 105,

            [Display(Name = "Single Value")]
            SingleValue = 106,

            [Display(Name = "Runchart")]
            Runchart = 107,

            [Display(Name = "Curve")]
            Curve = 108,

            [Display(Name = "Radar")]
            Radar = 109,

            [Display(Name = "Pareto")]
            Pareto = 110,

            [Display(Name = "Histogram")]
            Histogram = 111,

            [Display(Name = "Polar")]
            Polar = 112,

            [Display(Name = "Bullet")]
            Bullet = 113,

            [Display(Name = "Timeline")]
            Timeline = 114,

            [Display(Name = "Circle Progress")]
            CircleProgress = 115,

            [Display(Name = "Multi Value")]
            MultiValue = 116,

            [Display(Name = "Stacked")]
            Stacked = 117,

            [Display(Name = "Pyramid")]
            Pyramid = 118,

            [Display(Name = "Donut")]
            Donut = 119,

            [Display(Name = "Semi Circle")]
            SemiCircle = 120,

            [Display(Name = "Variance")]
            Variance = 121,

            [Display(Name = "Stacked Horizental")]
            StackedHorizental = 122,

            [Display(Name = "Bar Horizental")]
            BarHorizental = 123,

            [Display(Name = "Bar Combined")]
            BarCombined = 124,
        }

      
        public enum MonitorSize
        {
            LG = 1,
            MD = 2,
            SM = 3,
            XS = 4,
            XXS = 5
        }

     
        public enum IndexType
        {
            [Display(Name = "Negative")]
            Negative = 101,

            [Display(Name = "Positive")]
            Positive = 102,

            [Display(Name = "Neutral")]
            Neutral = 103,
        }

      
        public enum CommandType
        {
            Select = 1,
            View = 2,
            StoredProcedure = 3,
        }


        public enum OrganizationLevel
        {
            [Display(Name = "Organization Unit")]
            OrganizationUnit = 2,

            [Display(Name = "Discipline")]
            Discipline = 3,


            [Display(Name = "Project Discipline Per Project")]
            ProjectDisciplinePerProject = 5,

        }


        public enum IndexTemplateType
        {
            [Display(Name = "Organization")]
            Organization = 1,

            [Display(Name = "Organization Unit")]
            OrganizationUnit = 2,

            [Display(Name = "Discipline")]
            Discipline = 3,

            [Display(Name = "Project")]
            Project = 4,

            [Display(Name = "Project Discipline Per Project")]
            ProjectDisciplinePerProject = 5,

            [Display(Name = "Project Discipline Per Discipline")]
            ProjectDisciplinePerDiscipline = 6,
        }

        
        public enum Season
        {
            [Display(Name = "Spring")]
            Spring = 1,

            [Display(Name = "Summer")]
            Summer = 2,

            [Display(Name = "Autumn")]
            Autumn = 3,

            [Display(Name = "Winter")]
            Winter = 4,
        }

        public enum UserActionType
        {
            [Display(Name = "Click")]
            Click = 1,

            [Display(Name = "DoubleClick")]
            DoubleClick = 2,
        }

        public enum UserActionShowType
        {
            [Display(Name = "Total All Items")]
            TotalAllItems = 1,

            [Display(Name = "Total With Item")]
            TotalWithItem = 2,
        }

        public enum TimeDuration
        {
            [Display(Name = "Current Duration")]
            CurrentDuration = 1,

            [Display(Name = "Current Duration Then Prev Duration")]
            CurrentDurationThenPrevDuration = 2,

            [Display(Name = "Current Year")]
            CurrentYear = 3,

            [Display(Name = "Current Year Than Prev Year")]
            CurrentYearThanPrevYear = 4,
        }

        public enum CalcOperator
        {
            [Display(Name = "Equal")]
            Equal = 1,

            [Display(Name = "Less Than")]
            LessThan = 2,

            [Display(Name = "Less Than Equals")]
            LessThanEquals = 3,

            [Display(Name = "Greater than")]
            GreaterThan = 4,

            [Display(Name = "Greater Than Equals")]
            GreaterThanEquals = 5,
        }

       
        public enum OptimalCriteriaType
        {
            [Display(Name = "Fixed")]
            Fixed = 1,

            [Display(Name = "Parametric")]
            Parametric = 2,

            //[Display(Name = "براساس دوره زمانی")]
            //PerPeriod = 3,

            //[Display(Name = "براساس پروژه")]
            //PerProject = 4,
        }

        /// <summary>
        /// نوع کاربر
        /// </summary>
        public enum UserType
        {
            [Display(Name = "Customer User")]
            Customer = 1,

            [Display(Name = "Admin User")]
            Admin = 2,
        }
    }
