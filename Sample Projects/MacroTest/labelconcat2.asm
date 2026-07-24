* = $2000

flag_zero_is_set = 5

_i_f_s_t_a_c_k_17

!macro _P_u_s_h_I_f_S_t_a_c_k_
  _i_f_s_t_a_c_k_p_t_r_ = 17
  !end

!macro _i_f_f_l_a_g_ condition {

  +_P_u_s_h_I_f_S_t_a_c_k_
  ._n_u_m_ = _i_f_s_t_a_c_k_##_i_f_s_t_a_c_k_p_t_r_

  !if condition = flag_zero_is_set {
    bne _i_f_l_a_b_e_l_##._n_u_m_ ; <------ DAS ist das Problem, ging vorher problemlos.

    ; hier würden dann noch alle anderen conditions stehen, habe ich mal weggelassen

  } else {
    ;!error "ERROR: invalid condition"
  }

}

+_i_f_f_l_a_g_ 12