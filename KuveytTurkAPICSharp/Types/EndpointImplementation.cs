

namespace KuveytTurkAPICSharp.Types
{
    public class EndpointImplementation
    {
        /// <summary>
        /// Defines account list endpoint.
        /// </summary>
        public Endpoint AccountList
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/accounts/{suffix?}",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Account Transactions endpoint.
        /// </summary>
        public Endpoint AccountTransactions
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/accounts/{suffix?}/transactions",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Loan/Finance List endpoint.
        /// </summary>
        public Endpoint LoanFinanceList
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/loans",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Loan/Finance Info endpoint.
        /// </summary>
        public Endpoint LoanFinanceInfo
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/loans/{projectNumber}",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Loan/Finance Installments endpoint.
        /// </summary>
        public Endpoint LoanFinanceInstallments
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/loans/{projectNumber}/installments",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Money Transfer Execute endpoint.
        /// </summary>
        public Endpoint MoneyTransferExecute
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/transfers/execute",
                    MethodType = MethodType.Post,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Money Transfer to IBAN endpoint.
        /// </summary>
        public Endpoint MoneyTransferToIBAN
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/transfers/toIBAN",
                    MethodType = MethodType.Post,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Money Transfer to Account endpoint.
        /// </summary>
        public Endpoint MoneyTransferToAccount
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/transfers/toAccount",
                    MethodType = MethodType.Post,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Money Transfer to Name endpoint.
        /// </summary>
        public Endpoint MoneyTransferToName
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/transfers/toName",
                    MethodType = MethodType.Post,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Money Transfer to GSM endpoint.
        /// </summary>
        public Endpoint MoneyTransferToGSM
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/transfers/toGSM",
                    MethodType = MethodType.Post,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Money Transfer to GSM Transactions endpoint.
        /// </summary>
        public Endpoint MoneyTransferToGSMTransactions
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/transfers/toGSM/transactions",
                    MethodType = MethodType.Post,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Money Transfer to GSM Canceling endpoint.
        /// </summary>
        public Endpoint MoneyTransferToGSMCanceling
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/transfers/toGSM/cancel",
                    MethodType = MethodType.Post,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines Cash Withdrawal from ATM via QR Code endpoint.
        /// </summary>
        public Endpoint CashWithdrawalFromATMviaQRCode
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/transfers/fromATMByQRCode",
                    MethodType = MethodType.Post,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines	Test Customer List endpoint.
        /// </summary>
        public Endpoint TestCustomerList
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/data/testcustomers",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.ClientCredential
                };
            }
        }
        /// <summary>
        /// Defines	Loan/Finance Calculation endpoint.
        /// </summary>
        public Endpoint LoanFinanceCalculation
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/calculations/loan",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.ClientCredential
                };
            }
        }
        /// <summary>
        /// Defines	Loan/Finance Calculation Parameter endpoint.
        /// </summary>
        public Endpoint LoanFinanceCalculationParameter
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/data/loans",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.ClientCredential
                };
            }
        }
        /// <summary>
        /// Defines	FX Currency List endpoint.
        /// </summary>
        public Endpoint FXCurrencyList
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/data/fecs",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.ClientCredential
                };
            }
        }
        /// <summary>
        /// Defines	FX Currency Rates endpoint.
        /// </summary>
        public Endpoint FXCurrencyRates
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/fx/rates",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.ClientCredential
                };
            }
        }
        /// <summary>
        /// Defines	Kuveyt Turk Branch List endpoint.
        /// </summary>
        public Endpoint KuveytTurkBranchList
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/data/branches",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.ClientCredential
                };
            }
        }
        /// <summary>
        /// Defines	Kuveyt Turk ATM List endpoint.
        /// </summary>
        public Endpoint KuveytTurkATMList
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/data/atms",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.ClientCredential
                };
            }
        }
        /// <summary>
        /// Defines	Kuveyt Turk XTM List endpoint.
        /// </summary>
        public Endpoint KuveytTurkXTMList
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/data/xtms",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.ClientCredential
                };
            }
        }
        /// <summary>
        /// Defines	Bank List endpoint.
        /// </summary>
        public Endpoint BankList
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/data/banks",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.ClientCredential
                };
            }
        }
        /// <summary>
        /// Defines	Bank Branch List endpoint.
        /// </summary>
        public Endpoint BankBranchList
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/data/banks/{bankId}/branches?cityId={cityId}",
                    MethodType = MethodType.Get,
                    GrantType = GrantType.ClientCredential
                };
            }
        }
        /// <summary>
        /// Defines	User OTP Send endpoint.
        /// </summary>
        public Endpoint UserOTPSend
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/user/otp",
                    MethodType = MethodType.Post,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
        /// <summary>
        /// Defines	User OTP Verify endpoint.
        /// </summary>
        public Endpoint UserOTPVerify
        {
            get
            {
                return new Endpoint
                {
                    Path = "/v1/user/otp/verify",
                    MethodType = MethodType.Post,
                    GrantType = GrantType.AuthorizationCode
                };
            }
        }
    }
}
