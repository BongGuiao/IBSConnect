import { NgForm, FormGroup, FormControl, Validators, ValidatorFn, AbstractControl, ValidationErrors } from "@angular/forms";



/**
 * Returns a form validator that requires the values of the two control names must be equal. The error will be set on the other control
 * @param otherControlName
 */
export function mustMatchInGroup(controlName: string, otherControlName: string): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const control = group.get(controlName);
    const otherControl = group.get(otherControlName);

    if (otherControl.errors && !otherControl.errors.mustMatch) {
      // return if another validator has already found an error on the matchingControl
      return;
    }

    // set error on matchingControl if validation fails
    if (control.value !== otherControl.value) {
      control.setErrors({ mustMatch: true });
      return { mustMatch: true };
    } else {
      control.setErrors(null);
      return null;
    }
  }
}


/**
 * Returns a form validator that requires the values of the two control names must not be equal. The error will be set on the other control
 * @param controlName
 * @param otherControlName
 */
export function mustNotMatchInGroup(controlName: string, otherControlName: string): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const control = group.get(controlName);
    const otherControl = group.get(otherControlName);

    if (otherControl.errors && !otherControl.errors.mustMatch) {
      // return if another validator has already found an error on the matchingControl
      return;
    }

    // set error on matchingControl if validation fails
    if (control.value === otherControl.value) {
      control.setErrors({ mustNotMatch: true });
      return { mustNotMatch: true };
    } else {
      control.setErrors(null);
      return null;
    }
  }
}

/**
 * Returns a form validator that requires 
 * @param controlName
 * @param condition
 */
 export function requiredInGroupIf(controlName: string, condition: () => boolean): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const control = group.get(controlName);

    // set error on matchingControl if validation fails
    if (condition()) {
      return Validators.required(control);
    }
    return null;
  }
}


/**
 * Returns a form validator that requires the values of the two control names must be equal. The error will be set on the other control
 * @param otherControlName
 */
export function mustMatch(otherControlName: string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.parent) return null;

    const otherControl = control.parent.get(otherControlName);

    if (!otherControl) return null;

    // if (otherControl.errors && !otherControl.errors.mustMatch) {
    //   // return if another validator has already found an error on the matchingControl
    //   return;
    // }

    // set error on matchingControl if validation fails
    if (control.value !== otherControl.value) {
      otherControl.setErrors({ mustMatch: true });
      return { mustMatch: true };
    } else {
      otherControl.setErrors(null);
      return null;
    }
  }
}

/**
 * Returns a form validator that requires 
 * @param controlName
 * @param condition
 */
export function requiredIf(condition: () => boolean): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const control = formGroup;

    // set error on matchingControl if validation fails
    if (condition) {
      return Validators.required(control);
    }
  }
}

/**
 * Returns a form validator that requires the values of the two control names must not be equal. The error will be set on the other control
 * @param controlName
 * @param otherControlName
 */
export function mustNotMatch(otherControlName: string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.parent) return null;

    const otherControl = control.parent.get(otherControlName);

    if (!otherControl) return null;

    // if (otherControl.errors && !otherControl.errors.mustMatch) {
    //   // return if another validator has already found an error on the matchingControl
    //   return;
    // }

    if (control.value == otherControl.value) {
      return { mustNotMatch: true }
    }

    return null;
  }
}

/**
 * Returns a form validator that aggregates multiple form validators
 * @param functions
 */
export function aggregate(validators: ((formGroup: FormGroup) => void)[]) {
  return (formGroup: FormGroup) => {
    for (let validator of validators) {
      validator(formGroup);
    }
  };
}

/**
 * Validates that the control value should contain a mix of uppercase, lowercase and number characters
 * @param c
 */
export function validatePassword(c: FormControl) {
  let upperRegex = /[A-Z]/g;
  let lowerRegex = /[a-z]/g;
  let numberRegex = /[0-9]/g;

  let isValid = upperRegex.test(c.value) && lowerRegex.test(c.value) && numberRegex.test(c.value);

  return isValid ? null : {
    validatePassword: {
      invalid: true
    }
  };
}


declare module "@angular/forms" {
  interface FormGroup {
    resetValidation(this: FormGroup): void;
  }
}

FormGroup.prototype.resetValidation = function (this: FormGroup) {
  Object.keys(this.controls).forEach((key) => {
    const control = this.controls[key];
    control.markAsPristine();
    control.markAsUntouched();
  });
}
